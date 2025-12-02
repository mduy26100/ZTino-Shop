using Application.Features.Products.DTOs.Products;

namespace Application.Features.Products.Queries.Products.GetAllProducts
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
}
