using Application.Features.Products.DTOs.Products;

namespace Application.Features.Products.Commands.Products.CreateProduct
{
    public record CreateProductCommand(UpsertProductDto Dto) : IRequest<UpsertProductDto>;
}
