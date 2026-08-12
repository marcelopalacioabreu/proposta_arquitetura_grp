using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Retaguarda.Persistencia.POSTGRESQL
{
    /// <summary>
    /// Design-time factory for creating ApplicationDbContext instances without requiring a live HTTP context.
    /// Used by EF Core migrations tooling (dotnet ef migrations add, dotnet ef database update, etc.)
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var postgresConnection = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"]
                ?? "Host=localhost;Port=5432;Database=grp_banco_01;Username=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(postgresConnection,
                b => b.MigrationsAssembly("Retaguarda.Persistencia"));

            // Create context without IHttpContextAccessor (pass null for design-time)
            return new ApplicationDbContext(optionsBuilder.Options, null);
        }
    }
}
