using Application.Features.Products.Repositories;
using Domain.Models.Products;
using Infrastructure.Common.Interfaces.Persistence.Base;
using Infrastructure.Data;

namespace Infrastructure.Products.Repositories
{
    public class ProductImageRepository : Repository<ProductImage, int>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetMaxDisplayOrderAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductImages
                .Where(x => x.ProductVariantId == productVariantId)
                .Select(x => (int?)x.DisplayOrder)
                .MaxAsync(cancellationToken)
                ?? 0;
        }

        public async Task UnsetMainImageAsync(int productVariantId, CancellationToken cancellationToken = default)
        {
            var mainImages = await _context.ProductImages
                .Where(x => x.ProductVariantId == productVariantId && x.IsMain)
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
