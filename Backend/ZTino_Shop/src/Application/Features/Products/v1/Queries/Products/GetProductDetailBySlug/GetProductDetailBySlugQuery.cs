using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetailBySlug
{
    public record GetProductDetailBySlugQuery(string slug) : IRequest<ProductDetailDto?>;
}
