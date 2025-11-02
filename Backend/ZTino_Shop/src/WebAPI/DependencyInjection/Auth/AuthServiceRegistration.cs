using Infrastructure.Auth.Models;
using Infrastructure.Data;

namespace WebAPI.DependencyInjection.Auth
{
    public static class AuthServiceRegistration
    {
        public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            // ===== Identity =====
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
