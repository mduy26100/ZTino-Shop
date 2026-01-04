using Application.Features.Carts.v1.Repositories;
using Infrastructure.Carts.Repositories;

namespace WebAPI.DependencyInjection.Features
{
    public static class CartsServiceRegistration
    {
        public static IServiceCollection AddCartServices(this IServiceCollection services)
        {
            // Repository
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            return services;
        }
    }
}
