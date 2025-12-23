using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies
{
    public interface IProductImageUpdateStrategy
    {
        Task ExecuteAsync(ProductImage entity, IProductImageRepository productImageRepository, IApplicationDbContext context, CancellationToken cancellationToken);
    }
}
