using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Services
{
    public interface IProductQueryService
    {
        Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken);
        Task<ProductDetailDto?> GetProductDetailBySlugAsync(string slug, CancellationToken cancellationToken);
    }
}
