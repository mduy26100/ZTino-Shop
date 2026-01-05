using Application.Features.Orders.v1.Repositories;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderRepository : Repository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}