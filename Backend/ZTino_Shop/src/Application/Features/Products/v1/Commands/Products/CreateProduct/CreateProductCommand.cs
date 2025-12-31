using Application.Features.Products.v1.DTOs.Products;

namespace Application.Features.Products.v1.Commands.Products.CreateProduct
{
    public record CreateProductCommand(UpsertProductDto Dto) : IRequest<UpsertProductDto>;
}
