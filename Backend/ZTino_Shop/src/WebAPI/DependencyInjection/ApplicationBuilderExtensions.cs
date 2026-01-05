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
            await SeedIdentityData.SeedAsync(services);
        }
    }
}