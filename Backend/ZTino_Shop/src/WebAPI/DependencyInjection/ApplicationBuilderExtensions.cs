using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeds;

namespace WebAPI.DependencyInjection
{
    /// <summary>
    /// Extension methods for WebApplication (middleware pipeline configuration).
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Seeds initial data (roles, admin user, etc.) on application startup.
        /// </summary>
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                Console.WriteLine("--> Checking and applying pending migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("--> Migrations applied successfully!");

                await SeedIdentityData.SeedAsync(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error during startup: {ex.Message}");
            }
        }
    }
}