using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreApp.Repository.EFCore.DataContext;

namespace OnlineStoreApp.Repository.EFCore.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
        //public static void ApplyMigrations(this IApplicationBuilder app, int maxRetries = 10, int delaySeconds = 5)
        //{
        //    using IServiceScope scope = app.ApplicationServices.CreateScope();

        //    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //    for (int attempt = 1; attempt <= maxRetries; attempt++)
        //    {
        //        try
        //        {
        //            logger.LogInformation("⏳ Ejecutando migraciones (intento {Attempt}/{Total})…", attempt, maxRetries);
        //            dbContext.Database.Migrate();
        //            logger.LogInformation("✅ Migraciones aplicadas correctamente.");
        //            return;
        //        }
        //        catch (SqlException ex)
        //        {
        //            logger.LogWarning(ex,
        //                "SQL Server no disponible aún (intento {Attempt}/{Total}). Esperando {Delay}s...",
        //                attempt, maxRetries, delaySeconds);

        //            if (attempt == maxRetries)
        //            {
        //                logger.LogError("❌ No se pudo conectar a SQL Server tras {Total} intentos. Abortando.", maxRetries);
        //                throw;
        //            }

        //            Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
        //        }
        //    }
        //}
    }


}
