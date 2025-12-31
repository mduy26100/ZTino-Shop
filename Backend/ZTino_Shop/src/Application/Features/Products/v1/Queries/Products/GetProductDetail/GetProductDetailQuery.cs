using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetail
{
    public record GetProductDetailQuery(int id) : IRequest<ProductDetailDto?>;
}
