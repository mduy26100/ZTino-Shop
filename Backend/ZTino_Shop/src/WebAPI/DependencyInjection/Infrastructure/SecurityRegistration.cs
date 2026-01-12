using Application.Common.Abstractions.Security;
using Infrastructure.Security;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class SecurityRegistration
    {
        public static IServiceCollection AddInfrastructureSecurity(this IServiceCollection services)
        {
            services.AddSingleton<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
