using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Persistence;

namespace Infrastructure.Products.Repositories
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}