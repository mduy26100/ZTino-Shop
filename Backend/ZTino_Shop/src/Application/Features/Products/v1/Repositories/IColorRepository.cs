using Application.Common.Interfaces.Persistence.Base;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IColorRepository : IRepository<Color, int>
    {
    }
}
