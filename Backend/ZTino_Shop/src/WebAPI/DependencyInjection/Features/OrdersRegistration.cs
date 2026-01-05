using Application.Features.Orders.v1.Repositories;
using Infrastructure.Orders.Repositories;

namespace WebAPI.DependencyInjection.Features
{
    public static class OrdersRegistration
    {
        public static IServiceCollection AddOrdersFeature(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderAddressRepository, OrderAddressRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderPaymentRepository, OrderPaymentRepository>();
            services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();

            return services;
        }
    }
}
