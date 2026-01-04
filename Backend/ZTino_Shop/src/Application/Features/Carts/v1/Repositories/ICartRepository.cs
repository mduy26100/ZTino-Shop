using Application.Common.Interfaces.Persistence.Base;
using Domain.Models.Carts;

namespace Application.Features.Carts.v1.Repositories
{
    public interface ICartRepository : IRepository<Cart, Guid>
    {
    }
}
