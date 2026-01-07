using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Infrastructure.Orders.Repositories;
using Infrastructure.Orders.Services;

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

            // Services
            services.AddScoped<IOrderQueryService, OrderQueryService>();

            return services;
        }
    }
}
