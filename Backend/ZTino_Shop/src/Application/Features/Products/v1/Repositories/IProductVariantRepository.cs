using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductVariantRepository : IRepository<ProductVariant, int>
    {
        /// <summary>
        /// Gets a ProductVariant with all navigation properties needed for creating OrderItem:
        /// ProductColor, ProductColor.Product, ProductColor.Product.Category, ProductColor.Color, ProductColor.Images, Size
        /// </summary>
        Task<ProductVariant?> GetWithDetailsForOrderAsync(int id, CancellationToken cancellationToken = default);
    }
}