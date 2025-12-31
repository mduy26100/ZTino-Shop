using Application.Common.Interfaces.Persistence.Base;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage, int>
    {
        Task<int> GetMaxDisplayOrderAsync(int productVariantId, CancellationToken cancellationToken = default);
        Task UnsetMainImageAsync(int productVariantId, CancellationToken cancellationToken = default);
    }
}
