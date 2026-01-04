using Application.Features.Carts.v1.Repositories;
using Domain.Models.Carts;
using Infrastructure.Persistence;

namespace Infrastructure.Carts.Repositories
{
    public class CartRepository : Repository<Cart, Guid>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


