using Application.Features.Orders.v1.Repositories;
using Domain.Constants;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderRepository : Repository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> HasPreviousDeliveredOrdersAsync(
            Guid? userId,
            Guid excludeOrderId,
            CancellationToken cancellationToken = default)
        {
            if (!userId.HasValue)
            {
                return false; // Guest users are always "new"
            }

            return await _dbSet
                .AsNoTracking()
                .AnyAsync(o =>
                    o.UserId == userId.Value &&
                    o.Id != excludeOrderId &&
                    o.Status == OrderStatus.Delivered,
                    cancellationToken);
        }

        public async Task<Order?> GetWithDetailsForUpdateAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .Include(o => o.Invoice)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<Order?> GetByIdAndUserIdAsync(
            Guid orderId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .Include(o => o.Payments)
                .Include(o => o.Invoice)
                .FirstOrDefaultAsync(
                    o => o.Id == orderId && o.UserId == userId,
                    cancellationToken);
        }
    }
}