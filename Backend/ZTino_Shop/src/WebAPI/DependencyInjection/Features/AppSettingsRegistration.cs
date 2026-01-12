using Application.Features.Appsettings.v1.Repositories;
using Infrastructure.AppSettings.Repositories;

namespace WebAPI.DependencyInjection.Features
{
    public static class AppSettingsRegistration
    {
        public static IServiceCollection AddAppSettingsFeature(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IAppSettingRepository, AppSettingRepository>();

            return services;
        }
    }
}
