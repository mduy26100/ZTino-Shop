using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Repositories
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<bool> HasPreviousDeliveredOrdersAsync(Guid? userId, Guid excludeOrderId, CancellationToken cancellationToken = default);
        Task<Order?> GetWithDetailsForUpdateAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}