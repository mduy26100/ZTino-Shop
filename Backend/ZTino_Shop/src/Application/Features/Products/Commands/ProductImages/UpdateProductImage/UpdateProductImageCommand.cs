using Application.Features.Products.DTOs.ProductImages;

namespace Application.Features.Products.Commands.ProductImages.UpdateProductImage
{
    public record UpdateProductImageCommand(UpsertProductImageDto Dto) : IRequest<UpsertProductImageDto>;
}
