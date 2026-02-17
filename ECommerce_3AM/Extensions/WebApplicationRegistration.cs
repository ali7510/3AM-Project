using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.Data.DBcontexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_3AM.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataInitializer.InitializeAsync();
            return app;
        }
    }
}
