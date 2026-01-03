using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductColorId
{
    public record GetProductImagesByProductColorIdQuery(int productColorId) : IRequest<IEnumerable<ProductImageDto>>;
}