using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Persistence;

namespace Infrastructure.Products.Repositories
{
    public class ProductVariantRepository : Repository<ProductVariant, int>, IProductVariantRepository
    {
        public ProductVariantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProductVariant?> GetWithDetailsForOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(v => v.Size)
                .Include(v => v.ProductColor)
                    .ThenInclude(pc => pc.Color)
                .Include(v => v.ProductColor)
                    .ThenInclude(pc => pc.Product)
                        .ThenInclude(p => p.Category)
                .Include(v => v.ProductColor)
                    .ThenInclude(pc => pc.Images)
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
    }
}