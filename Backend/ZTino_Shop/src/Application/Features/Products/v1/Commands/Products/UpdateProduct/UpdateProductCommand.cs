using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Commands.Products.UpdateProduct
{
    public record UpdateProductCommand(UpsertProductDto Dto) : IRequest<UpsertProductDto>;
}
