using Domain.Models.Orders;

namespace Application.Features.Stats.v1.Services
{
    public interface ISalesStatsService
    {
        Task UpdateDailyRevenueStatsAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateProductSalesStatsAsync(ICollection<OrderItem> orderItems, CancellationToken cancellationToken = default);
    }
}
