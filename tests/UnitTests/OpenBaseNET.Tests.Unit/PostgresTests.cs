using System.Data;
using System.Diagnostics;
using System.Net.Http.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenBaseNET.Application.Customers;
using OpenBaseNET.Domain.Entities;
using OpenBaseNET.Infra.CrossCutting;
using OpenBaseNET.Infra.Dapper.Extension;
using OpenBaseNET.Infra.Data.Context;
using OpenBaseNET.Infra.Uow.Interfaces;

namespace OpenBaseNET.Tests.Unit;

public sealed class PostgresFactAttribute : FactAttribute
{
    public PostgresFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OPENBASE_TEST_POSTGRES")))
            Skip = "Set OPENBASE_TEST_POSTGRES to a PostgreSQL connection with CREATEDB permission.";
    }
}

public sealed class PostgresTests
{
    [PostgresFact]
    public async Task Ef_and_dapper_share_commit_rollback_and_support_a_new_transaction()
    {
        await using var database = await TestDatabase.CreateAsync();
        var services = new ServiceCollection();
        services.AddApplicationServices(new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?> { ["ConnectionStrings:OpenBasePostgres"] = database.ConnectionString }).Build());
        await using var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<OneBaseDataBaseContext>();
        await context.Database.MigrateAsync();
        var session = scope.ServiceProvider.GetRequiredService<DbSession>();
        var unit = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        Assert.Same(session.Connection, context.Database.GetDbConnection());

        await unit.BeginTransactionAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(() => unit.BeginTransactionAsync());
        context.Add(new Customer { Name = "EF rolled back" });
        await context.SaveChangesAsync();
        await InsertWithDapper(session, "Dapper rolled back");
        await unit.RollbackAsync();
        Assert.Null(session.Transaction);
        Assert.Null(context.Database.CurrentTransaction);
        Assert.Equal(0, await context.Set<Customer>().CountAsync());

        await unit.BeginTransactionAsync();
        context.Add(new Customer { Name = "EF committed" });
        await InsertWithDapper(session, "Dapper committed");
        await unit.CommitAsync(); // Also saves pending EF changes.
        Assert.Null(session.Transaction);
        Assert.Equal(2, await context.Set<Customer>().CountAsync());
        await Assert.ThrowsAsync<InvalidOperationException>(() => unit.CommitAsync());
    }

    [PostgresFact]
    public async Task Disposal_rolls_back_an_uncommitted_transaction()
    {
        await using var database = await TestDatabase.CreateAsync();
        await using var connection = new NpgsqlConnection(database.ConnectionString);
        var session = new DbSession(connection);
        await using var context = new OneBaseDataBaseContext(session);
        await context.Database.MigrateAsync();
        await using (var unit = new OpenBaseNET.Infra.Uow.UnitOfWork(session, context))
        {
            await unit.BeginTransactionAsync();
            await InsertWithDapper(session, "Discarded");
        }
        Assert.Null(session.Transaction);
        Assert.Equal(0, await context.Set<Customer>().CountAsync());
    }

    [PostgresFact]
    public async Task Dapper_cancels_an_in_flight_database_command()
    {
        await using var database = await TestDatabase.CreateAsync();
        await using var connection = new NpgsqlConnection(database.ConnectionString);
        await connection.OpenAsync();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(250));
        var elapsed = Stopwatch.StartNew();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => connection.ExecuteAsyncWithRetry(
            cancellation.Token, "SELECT pg_sleep(10)", commandTimeout: 15));
        Assert.True(elapsed.Elapsed < TimeSpan.FromSeconds(5), "Cancellation did not reach PostgreSQL.");
        Assert.Equal(1, await connection.ExecuteScalarAsync<int>("SELECT 1"));
    }

    [PostgresFact]
    public async Task Customer_api_creates_reads_and_lists_using_postgresql()
    {
        await using var database = await TestDatabase.CreateAsync();
        await using var factory = new DatabaseApiFactory(database.ConnectionString);
        using var client = factory.CreateClient(new() { BaseAddress = new Uri("https://localhost") });
        await using (var scope = factory.Services.CreateAsyncScope())
            await scope.ServiceProvider.GetRequiredService<OneBaseDataBaseContext>().Database.MigrateAsync();
        var created = await client.PostAsJsonAsync("/api/customers", new CreateCustomer("Ada Lovelace"));
        Assert.Equal(System.Net.HttpStatusCode.Created, created.StatusCode);
        var customer = await created.Content.ReadFromJsonAsync<CustomerResponse>();
        Assert.NotNull(customer);
        var loaded = await client.GetFromJsonAsync<CustomerResponse>(created.Headers.Location);
        Assert.Equal(customer, loaded);
        var page = await client.GetFromJsonAsync<List<CustomerResponse>>("/api/customers?pageNumber=1&pageSize=10");
        Assert.Single(page!);
        var invalid = await client.GetAsync("/api/customers?pageNumber=0&pageSize=10");
        Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, invalid.StatusCode);
    }

    private static Task<int> InsertWithDapper(DbSession session, string name) =>
        session.Connection.ExecuteAsyncWithRetry(default,
            "INSERT INTO \"Customers\" (\"Id\", \"Name\") VALUES (@Id, @Name)",
            transaction: session.Transaction, parameters: new { Id = Guid.NewGuid(), Name = name });

    private sealed class DatabaseApiFactory(string connectionString) : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config => config.AddInMemoryCollection(
                new Dictionary<string, string?> { ["ConnectionStrings:OpenBasePostgres"] = connectionString }));
            return base.CreateHost(builder);
        }
    }

    private sealed class TestDatabase(string adminConnectionString, string name, string connectionString) : IAsyncDisposable
    {
        public string ConnectionString { get; } = connectionString;
        public static async Task<TestDatabase> CreateAsync()
        {
            var admin = Environment.GetEnvironmentVariable("OPENBASE_TEST_POSTGRES")!;
            var name = "openbase_test_" + Guid.NewGuid().ToString("N");
            await using var connection = new NpgsqlConnection(admin);
            await connection.OpenAsync();
            await connection.ExecuteAsync($"CREATE DATABASE {name}");
            var builder = new NpgsqlConnectionStringBuilder(admin) { Database = name, Pooling = false };
            return new TestDatabase(admin, name, builder.ConnectionString);
        }

        public async ValueTask DisposeAsync()
        {
            await using var connection = new NpgsqlConnection(adminConnectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync($"DROP DATABASE {name} WITH (FORCE)");
        }
    }
}
