using Application.Features.Carts.v1.Repositories;
using Domain.Models.Carts;
using Infrastructure.Common.Interfaces.Persistence.Base;
using Infrastructure.Data;

namespace Infrastructure.Carts.Repositories
{
    public class CartItemRepository : Repository<CartItem, int>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
