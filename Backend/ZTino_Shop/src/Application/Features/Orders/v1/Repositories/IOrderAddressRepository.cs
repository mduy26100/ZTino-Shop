using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Repositories
{
    public interface IOrderAddressRepository : IRepository<OrderAddress, int>
    {
    }
}