using Application.Common.Interfaces.Persistence.Base;
using Domain.Models.Products;

namespace Application.Features.Products.Repositories
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
    }
}
