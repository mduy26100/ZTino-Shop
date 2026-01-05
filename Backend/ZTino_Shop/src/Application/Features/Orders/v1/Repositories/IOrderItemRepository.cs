using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem, int>
    {
    }
}
