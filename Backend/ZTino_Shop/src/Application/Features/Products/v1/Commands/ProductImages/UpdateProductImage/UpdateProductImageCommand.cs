using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage
{
    public record UpdateProductImageCommand(UpsertProductImageDto Dto) : IRequest<UpsertProductImageDto>;
}
