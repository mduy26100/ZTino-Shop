using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage, int>
    {
        Task<int> GetMaxDisplayOrderAsync(int productColorId, CancellationToken cancellationToken = default);
        Task UnsetMainImageAsync(int productColorId, CancellationToken cancellationToken = default);
    }
}