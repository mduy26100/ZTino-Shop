using Application.Common.Interfaces.Persistence.Caching;
using Infrastructure.Data.Caching;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class CacheServiceRegistration
    {
        public static IServiceCollection AddCacheServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Redis:ConnectionString");
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
