using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductVariantRepository : IRepository<ProductVariant, int>
    {
        Task<ProductVariant?> GetWithDetailsForOrderAsync(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ProductVariant>> GetManyWithDetailsForOrderAsync(
            IEnumerable<int> ids,
            CancellationToken cancellationToken = default);
    }
}