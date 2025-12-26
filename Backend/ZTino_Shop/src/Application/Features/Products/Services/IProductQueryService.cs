using Application.Features.Products.DTOs.Products;

namespace Application.Features.Products.Services
{
    public interface IProductQueryService
    {
        Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken);
    }
}
