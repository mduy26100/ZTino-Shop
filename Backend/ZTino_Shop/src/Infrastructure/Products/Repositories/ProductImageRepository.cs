using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Persistence;

namespace Infrastructure.Products.Repositories
{
    public class ProductImageRepository : Repository<ProductImage, int>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetMaxDisplayOrderAsync(int productColorId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductImages
                .Where(x => x.ProductColorId == productColorId)
                .Select(x => (int?)x.DisplayOrder)
                .MaxAsync(cancellationToken)
                ?? 0;
        }

        public async Task UnsetMainImageAsync(int productColorId, CancellationToken cancellationToken = default)
        {
            var mainImages = await _context.ProductImages
                .Where(x => x.ProductColorId == productColorId && x.IsMain)
                .ToListAsync(cancellationToken);

            if (!mainImages.Any())
                return;

            foreach (var image in mainImages)
            {
                image.IsMain = false;
            }
        }
    }
}