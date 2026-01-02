using Application.Features.Products.v1.DTOs.ProductColor;

namespace Application.Features.Products.v1.Commands.ProductColor.UpdateProductColor
{
    public record UpdateProductColorCommand(UpsertProductColorDto Dto) : IRequest<UpsertProductColorDto>;
}
