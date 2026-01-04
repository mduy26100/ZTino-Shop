using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}