using Application.Features.Products.DTOs.ProductImages;

namespace Application.Features.Products.Commands.ProductImages.CreateProductImage
{
    public record CreateProductImagesCommand(List<UpsertProductImageDto> Dtos) : IRequest<List<UpsertProductImageDto>>;
}
