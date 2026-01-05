using Application.Common.Abstractions.Caching;
using Infrastructure.Caching;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class CachingRegistration
    {
        public static IServiceCollection AddCaching(
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
