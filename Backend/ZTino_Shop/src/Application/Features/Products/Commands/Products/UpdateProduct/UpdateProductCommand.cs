using Application.Features.Products.DTOs.Products;

namespace Application.Features.Products.Commands.Products.UpdateProduct
{
    public record UpdateProductCommand(UpsertProductDto Dto) : IRequest<UpsertProductDto>;
}
