using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies
{
    public class ClaimMainStrategy : IProductImageUpdateStrategy
    {
        public async Task ExecuteAsync(ProductImage entity, IProductImageRepository productImageRepository, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var oldMainImages = await productImageRepository.FindAsync(
                x => x.ProductVariantId == entity.ProductVariantId && x.IsMain == true && x.Id != entity.Id,
                true, cancellationToken);

            if (oldMainImages.Any())
            {
                foreach (var old in oldMainImages)
                {
                    old.IsMain = false;
                    productImageRepository.Update(old);
                }
                await context.SaveChangesAsync(cancellationToken);
            }

            entity.IsMain = true;
            productImageRepository.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
