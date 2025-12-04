using Application.Features.Products.Mappings;

namespace WebAPI.DependencyInjection.Infrastructure
{
    public static class MappingServiceRegistration
    {
        public static IServiceCollection AddMappingServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CategoryMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<ColorMappingProfile>();
            });

            return services;
        }
    }
}
