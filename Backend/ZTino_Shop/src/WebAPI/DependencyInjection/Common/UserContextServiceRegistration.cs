using Application.Common.Abstractions.Identity;
using Infrastructure.Identity;

namespace WebAPI.DependencyInjection.Common
{
    public static class UserContextServiceRegistration
    {
        public static IServiceCollection AddUserContextServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            return services;
        }
    }
}


