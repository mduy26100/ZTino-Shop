using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies
{
    public class ResignMainStrategy : IProductImageUpdateStrategy
    {
        public async Task ExecuteAsync(ProductImage entity, IProductImageRepository productImageRepository, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            entity.IsMain = false;
            productImageRepository.Update(entity);
            await context.SaveChangesAsync(cancellationToken);

            var siblings = await productImageRepository.FindAsync(
                x => x.ProductVariantId == entity.ProductVariantId && x.Id != entity.Id,
                true, cancellationToken);

            var heir = siblings.FirstOrDefault();

            if (heir != null)
            {
                heir.IsMain = true;
                productImageRepository.Update(heir);
            }
            else
            {
                entity.IsMain = true;
                productImageRepository.Update(entity);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
