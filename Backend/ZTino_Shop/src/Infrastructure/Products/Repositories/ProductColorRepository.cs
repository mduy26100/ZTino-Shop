using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Persistence;

namespace Infrastructure.Products.Repositories
{
    public class ProductColorRepository : Repository<ProductColor, int>, IProductColorRepository
    {
        public ProductColorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}