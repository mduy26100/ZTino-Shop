using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Queries.Products.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
}
