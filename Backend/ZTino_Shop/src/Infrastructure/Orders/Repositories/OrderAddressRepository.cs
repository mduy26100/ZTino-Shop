using Application.Features.Orders.v1.Repositories;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderAddressRepository : Repository<OrderAddress, int>, IOrderAddressRepository
    {
        public OrderAddressRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}