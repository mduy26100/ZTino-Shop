using Application.Features.Products.v1.DTOs.ProductColor;

namespace Application.Features.Products.v1.Commands.ProductColor.CreateProductColor
{
    public record CreateProductColorCommand(UpsertProductColorDto Dto) : IRequest<UpsertProductColorDto>;
}
