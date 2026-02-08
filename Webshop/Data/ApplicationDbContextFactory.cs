using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Webshop.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Konfiguration aus appsettings.json und User Secrets laden
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddUserSecrets<ApplicationDbContextFactory>()
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Datenbank-Provider und Connection String aus Konfiguration
            var databaseProvider = configuration["DatabaseProvider"] ?? "SqlServer";
            var connectionStringName = databaseProvider.ToUpperInvariant() switch
            {
                "MYSQL" or "MARIADB" => "MySqlConnection",
                "SQLSERVER" or "MSSQL" => "SqlServerConnection",
                _ => throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}. Supported: MySQL, MariaDB, SqlServer, MsSql")
            };

            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

            switch (databaseProvider.ToUpperInvariant())
            {
                case "MYSQL":
                case "MARIADB":
                    ConfigureMySql(optionsBuilder, connectionString);
                    break;
                case "SQLSERVER":
                case "MSSQL":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}. Supported: MySQL, MariaDB, SqlServer, MsSql");
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private static void ConfigureMySql(DbContextOptionsBuilder options, string connectionString)
        {
            var mySqlExtensionsType = Type.GetType("Microsoft.EntityFrameworkCore.MySqlDbContextOptionsBuilderExtensions, Pomelo.EntityFrameworkCore.MySql");
            var serverVersionType = Type.GetType("Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerVersion, Pomelo.EntityFrameworkCore.MySql");

            if (mySqlExtensionsType is null || serverVersionType is null)
            {
                throw new InvalidOperationException("MySQL provider requires the Pomelo.EntityFrameworkCore.MySql package. Add the package to enable MySQL.");
            }

            var autoDetect = serverVersionType.GetMethod("AutoDetect", new[] { typeof(string) });
            if (autoDetect is null)
            {
                throw new InvalidOperationException("Pomelo MySQL provider is missing the ServerVersion.AutoDetect method.");
            }

            var serverVersion = autoDetect.Invoke(null, new object[] { connectionString });
            var useMySql = mySqlExtensionsType.GetMethod("UseMySql", new[] { typeof(DbContextOptionsBuilder), typeof(string), serverVersionType });

            if (useMySql is null)
            {
                throw new InvalidOperationException("Pomelo MySQL provider is missing the UseMySql method.");
            }

            useMySql.Invoke(null, new[] { options, connectionString, serverVersion! });
        }
    }
}
