using Application.Features.Products.Interfaces.Services.Commands.Categories.CreateCategory;
using Application.Features.Products.Repositories;
using Application.Features.Products.Services.Commands.Categories.CreateCategory;
using Infrastructure.Products.Repositories;

namespace WebAPI.DependencyInjection.Features
{
    public static class ProductsServiceRegistration
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            // Repository
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();

            //Categories
            services.AddScoped<ICreateCategoryService, CreateCategoryService>();

            return services;
        }
    }
}
