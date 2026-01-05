using Application.Features.Orders.v1.Repositories;
using Domain.Models.Orders;
using Infrastructure.Persistence;

namespace Infrastructure.Orders.Repositories
{
    public class OrderPaymentRepository : Repository<OrderPayment, int>, IOrderPaymentRepository
    {
        public OrderPaymentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}