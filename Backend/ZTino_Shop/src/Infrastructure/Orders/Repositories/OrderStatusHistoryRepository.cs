using Application.Features.Orders.v1.Repositories;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderStatusHistoryRepository : Repository<OrderStatusHistory, int>, IOrderStatusHistoryRepository
    {
        public OrderStatusHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}