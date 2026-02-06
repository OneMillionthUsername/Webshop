using Microsoft.EntityFrameworkCore;
using Webshop.Data;
using Webshop.Services;

namespace Webshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Datenbank-Provider und Connection String laden
            var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "SqlServer";

            var connectionStringName = databaseProvider.ToUpperInvariant() switch
            {
                "MYSQL" or "MARIADB" => "MySqlConnection",
                "SQLSERVER" or "MSSQL" => "SqlServerConnection",
                _ => throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}. Supported: MySQL, MariaDB, SqlServer, MsSql")
            };

            var connectionString = builder.Configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");
            
            // EF Core DbContext mit dynamischem Provider registrieren
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                switch (databaseProvider.ToUpperInvariant())
                {
                    case "MYSQL":
                    case "MARIADB":
                        ConfigureMySql(options, connectionString);
                        break;
                    case "SQLSERVER":
                    case "MSSQL":
                        options.UseSqlServer(connectionString);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported database provider: {databaseProvider}. Supported: MySQL, MariaDB, SqlServer, MsSql");
                }
            });

            // HttpContextAccessor für Session/Cookie-Zugriff
            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Services registrieren (Dependency Injection)
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPricingService, PricingService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            var app = builder.Build();

            // Datenbank-Migrationen automatisch beim Start ausführen
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ein Fehler ist beim Migrieren der Datenbank aufgetreten.");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
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
