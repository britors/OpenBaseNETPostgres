using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using OpenBaseNET.Infra.Settings.ConnectionStrings;

namespace OpenBaseNET.Infra.Data.Context;

public class OneBaseDataBaseContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var cn = (configuration.GetConnectionString(OneBaseConnectionStrings.OpenBasePostgres));

        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(cn);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
