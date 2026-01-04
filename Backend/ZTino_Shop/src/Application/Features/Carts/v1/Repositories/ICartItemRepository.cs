using Application.Common.Interfaces.Persistence.Base;
using Domain.Models.Carts;

namespace Application.Features.Carts.v1.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem, int>
    {
    }
}
