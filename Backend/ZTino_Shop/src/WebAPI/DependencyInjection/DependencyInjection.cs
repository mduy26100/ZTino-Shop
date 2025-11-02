using WebAPI.DependencyInjection.Auth;
using WebAPI.DependencyInjection.Common;
using WebAPI.DependencyInjection.Infrastructure;

namespace WebAPI.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services
            , IConfiguration configuration)
        {
            //Auth
            services.AddAuthService(configuration);

            //Infrastructure
            services.AddDatabaseServices(configuration);


            //Common
            services.AddCorsPolicies();

            return services;
        }
    }
}
