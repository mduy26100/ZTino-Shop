using Application.Features.Products.DTOs.Products;

namespace Application.Features.Products.Queries.Products.GetProductDetail
{
    public record GetProductDetailQuery(int id) : IRequest<ProductDetailDto?>;
}
