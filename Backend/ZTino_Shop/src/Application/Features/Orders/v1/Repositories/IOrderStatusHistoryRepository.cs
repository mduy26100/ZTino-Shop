using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Repositories
{
    public interface IOrderStatusHistoryRepository : IRepository<OrderStatusHistory, int>
    {
    }
}