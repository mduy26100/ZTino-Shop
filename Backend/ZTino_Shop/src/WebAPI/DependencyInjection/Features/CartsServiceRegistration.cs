using Application.Features.Carts.v1.Repositories;
using Application.Features.Carts.v1.Services;
using Infrastructure.Carts.Repositories;
using Infrastructure.Carts.Services;

namespace WebAPI.DependencyInjection.Features
{
    public static class CartsServiceRegistration
    {
        public static IServiceCollection AddCartServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            // Services
            services.AddScoped<ICartQueryService, CartQueryService>();

            return services;
        }
    }
}
