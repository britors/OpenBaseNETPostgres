using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OpenBaseNET.Domain.Entities;
using OpenBaseNET.Infra.Data.Context;
using OpenBaseNET.Infra.EF.Extension;

namespace OpenBaseNET.Tests.Unit;

public sealed class PaginationTests
{
    [Fact]
    public async Task Pages_use_explicit_ordering_without_overlap()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = new SqliteContext(new DbSession(connection));
        await context.Database.EnsureCreatedAsync();
        context.AddRange(new Customer { Name = "Charlie" }, new Customer { Name = "Alice" }, new Customer { Name = "Bob" });
        await context.SaveChangesAsync();
        var first = await context.FindAsyncWithRetry<Customer>(default, pageNumber: 1, pageSize: 2,
            orderBy: query => query.OrderBy(customer => customer.Name).ThenBy(customer => customer.Id));
        var second = await context.FindAsyncWithRetry<Customer>(default, pageNumber: 2, pageSize: 2,
            orderBy: query => query.OrderBy(customer => customer.Name).ThenBy(customer => customer.Id));
        Assert.Equal(new[] { "Alice", "Bob" }, first.Select(customer => customer.Name));
        Assert.Equal(new[] { "Charlie" }, second.Select(customer => customer.Name));
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, null)]
    [InlineData(null, 20)]
    [InlineData(1, 20)]
    public async Task Invalid_or_unordered_pages_are_rejected(int? pageNumber, int? pageSize)
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await using var context = new SqliteContext(new DbSession(connection));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => context.FindAsyncWithRetry<Customer>(default,
            pageNumber: pageNumber, pageSize: pageSize));
    }

    private sealed class SqliteContext : OneBaseDataBaseContext
    {
        private readonly System.Data.Common.DbConnection connection;
        public SqliteContext(DbSession session) : base(session) => connection = session.Connection;
        protected override void OnConfiguring(DbContextOptionsBuilder builder) => builder.UseSqlite(connection);
    }
}
