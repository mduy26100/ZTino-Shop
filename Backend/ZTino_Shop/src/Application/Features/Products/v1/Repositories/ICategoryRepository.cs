using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
    }
}