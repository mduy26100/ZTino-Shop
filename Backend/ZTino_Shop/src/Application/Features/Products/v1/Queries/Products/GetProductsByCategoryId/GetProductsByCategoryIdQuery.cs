using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Queries.Products.GetProductsByCategoryId
{
    public record GetProductsByCategoryIdQuery(int CategoryId) : IRequest<List<ProductSummaryDto>>;
}
