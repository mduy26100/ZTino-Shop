using Domain.Models.Orders;

namespace Application.Features.Products.v1.Services
{
    /// <summary>
    /// Service for stock management operations.
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Restores stock quantities when an order is cancelled or returned.
        /// </summary>
        Task RestoreStockAsync(ICollection<OrderItem> orderItems, CancellationToken cancellationToken = default);
    }
}
