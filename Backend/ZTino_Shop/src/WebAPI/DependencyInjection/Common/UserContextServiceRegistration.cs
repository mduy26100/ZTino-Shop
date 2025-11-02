using Application.Common.Interfaces.Services.User;
using Application.Common.Interfaces.Shared;
using Infrastructure.Common.Interfaces.Services.User;
using Infrastructure.Common.Interfaces.Shared;

namespace WebAPI.DependencyInjection.Common
{
    public static class UserContextServiceRegistration
    {
        public static IServiceCollection AddUserContextServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            return services;
        }
    }
}
