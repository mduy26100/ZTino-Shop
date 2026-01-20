using Application.Features.Products.v1.Repositories;
using Application.Features.Products.v1.Services;
using Infrastructure.Products.Repositories;
using Infrastructure.Products.Services;

namespace WebAPI.DependencyInjection.Features
{
    public static class ProductsRegistration
    {
        public static IServiceCollection AddProductsFeature(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IProductColorRepository, ProductColorRepository>();

            // Services
            services.AddScoped<IProductQueryService, ProductQueryService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IStockService, StockService>();

            return services;

        }
    }
}
