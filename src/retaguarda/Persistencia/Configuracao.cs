using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Retaguarda.Persistencia
{
    public static class Configuracao
    {
        /// <summary>
        /// Registra o DbContext de persistência conforme as configurações (Postgres ou MySql).
        /// Chamar a partir do projeto de inicialização (ex.: Api) passando a IConfiguration.
        /// </summary>
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration["Persistence:Provider"] ?? "Postgres";
            var defaultConnection = configuration.GetConnectionString("DefaultConnection") ?? configuration["ConnectionStrings:DefaultConnection"];
            // Allow an explicit MySql connection string; fall back to constructing one from the default (Postgres) connection string
            var mysqlConnection = configuration.GetConnectionString("MySql") ?? configuration["ConnectionStrings:MySql"];
            string connectionString = defaultConnection;

            if (string.IsNullOrWhiteSpace(connectionString) && string.IsNullOrWhiteSpace(mysqlConnection))
                return services; // nothing to configure

            if (provider.Equals("Postgres", System.StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Retaguarda.Persistencia")));
                services.AddScoped<Retaguarda.Persistencia.IApplicationDbContext>(sp =>
                    sp.GetRequiredService<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>());
            }
            else if (provider.Equals("MySql", System.StringComparison.OrdinalIgnoreCase))
            {
                // If no explicit MySql connection is provided, try to derive the database name from the default connection string
                if (string.IsNullOrWhiteSpace(mysqlConnection) && !string.IsNullOrWhiteSpace(defaultConnection))
                {
                    string dbName = null;
                    var parts = defaultConnection.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in parts)
                    {
                        var idx = p.IndexOf('=');
                        if (idx <= 0) continue;
                        var key = p.Substring(0, idx).Trim();
                        var val = p.Substring(idx + 1).Trim();
                        if (key.Equals("Database", System.StringComparison.OrdinalIgnoreCase) || key.Equals("Initial Catalog", System.StringComparison.OrdinalIgnoreCase))
                        {
                            dbName = val;
                            break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dbName))
                    {
                        // Build a simple MySQL connection string using localhost/root/no password/3306 as requested
                        mysqlConnection = $"Server=localhost;Port=3306;Database={dbName};User=root;Password=;";
                    }
                }

                if (string.IsNullOrWhiteSpace(mysqlConnection))
                {
                    // Fall back to the default connection string if necessary
                    mysqlConnection = defaultConnection;
                }

                services.AddDbContext<Retaguarda.Persistencia.MYSQL.ApplicationDbContext>(options =>
                    options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection), b => b.MigrationsAssembly("Retaguarda.Persistencia")));
                services.AddScoped<Retaguarda.Persistencia.IApplicationDbContext>(sp =>
                    sp.GetRequiredService<Retaguarda.Persistencia.MYSQL.ApplicationDbContext>());
            }
            else
            {
                services.AddDbContext<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Retaguarda.Persistencia")));
                services.AddScoped<Retaguarda.Persistencia.IApplicationDbContext>(sp =>
                    sp.GetRequiredService<Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext>());
            }

            return services;
        }
    }
}
