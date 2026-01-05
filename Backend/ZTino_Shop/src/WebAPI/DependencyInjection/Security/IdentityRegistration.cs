using Application.Common.Abstractions.Identity;
using Infrastructure.Auth.Models;
using Infrastructure.Identity;
using Infrastructure.Persistence;

namespace WebAPI.DependencyInjection.Security
{
    public static class IdentityRegistration
    {
        public static IServiceCollection AddIdentityCore(
            this IServiceCollection services, IConfiguration configuration)
        {
            // ASP.NET Core Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // HTTP Context accessor for user claims
            services.AddHttpContextAccessor();

            // Identity abstractions
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
