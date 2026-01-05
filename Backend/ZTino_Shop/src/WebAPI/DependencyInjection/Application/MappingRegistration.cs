using Application.Features.Products.v1.Mappings;

namespace WebAPI.DependencyInjection.Application
{
    public static class MappingRegistration
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CategoryMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<ColorMappingProfile>();
                cfg.AddProfile<SizeMappingProfile>();
                cfg.AddProfile<ProductVariantMappingProfile>();
                cfg.AddProfile<ProductImageMappingProfile>();
                cfg.AddProfile<ProductColorMappingProfile>();
            });

            return services;
        }
    }
}