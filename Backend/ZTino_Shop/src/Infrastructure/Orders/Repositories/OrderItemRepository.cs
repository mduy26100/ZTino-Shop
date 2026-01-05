using Application.Features.Orders.v1.Repositories;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderItemRepository : Repository<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}