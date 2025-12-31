using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.Commands.ProductImages.CreateProductImages
{
    public record CreateProductImagesCommand(List<UpsertProductImageDto> Dtos) : IRequest<List<UpsertProductImageDto>>;
}
