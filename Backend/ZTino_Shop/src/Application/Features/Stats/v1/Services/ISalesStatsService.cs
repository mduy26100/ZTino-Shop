using Domain.Models.Orders;

namespace Application.Features.Stats.v1.Services
{
    /// <summary>
    /// Service for updating sales statistics.
    /// </summary>
    public interface ISalesStatsService
    {
        /// <summary>
        /// Updates daily revenue statistics when an order is delivered.
        /// </summary>
        Task UpdateDailyRevenueStatsAsync(Order order, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates product sales statistics when an order is delivered.
        /// Groups items by ProductId to aggregate quantities and revenues.
        /// </summary>
        Task UpdateProductSalesStatsAsync(ICollection<OrderItem> orderItems, CancellationToken cancellationToken = default);
    }
}
