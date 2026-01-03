using Application.Features.Products.v1.DTOs.ProductColor;

namespace Application.Features.Products.v1.Queries.ProductColors.GetColorsByProductId
{
    public record GetColorsByProductIdQuery(int productId) : IRequest<List<ProductColorSummaryDto>>;
}
