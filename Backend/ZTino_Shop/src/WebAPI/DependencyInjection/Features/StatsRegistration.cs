using Application.Features.Stats.v1.Repositories;
using Application.Features.Stats.v1.Services;
using Infrastructure.Stats.Repositories;
using Infrastructure.Stats.Services;

namespace WebAPI.DependencyInjection.Features
{
    public static class StatsRegistration
    {
        public static IServiceCollection AddStatsFeature(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IDailyRevenueStatsRepository, DailyRevenueStatsRepository>();
            services.AddScoped<IProductSalesStatsRepository, ProductSalesStatsRepository>();

            // Services
            services.AddScoped<ISalesStatsService, SalesStatsService>();

            return services;
        }
    }
}
