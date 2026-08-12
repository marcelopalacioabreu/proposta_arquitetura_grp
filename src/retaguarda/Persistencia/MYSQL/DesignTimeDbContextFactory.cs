using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Retaguarda.Persistencia.MYSQL
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

            var mysqlConnection = configuration.GetConnectionString("MySql")
                ?? configuration["ConnectionStrings:MySql"]
                ?? "Server=localhost;Port=3306;Database=grp_banco_01;User=root;Password=;";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection),
                b => b.MigrationsAssembly("Retaguarda.Persistencia"));

            // Create context without IHttpContextAccessor (pass null for design-time)
            return new ApplicationDbContext(optionsBuilder.Options, null);
        }
    }
}
