using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies
{
    public class DefaultUpdateStrategy : IProductImageUpdateStrategy
    {
        public async Task ExecuteAsync(ProductImage entity, IProductImageRepository productImageRepository, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            productImageRepository.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
