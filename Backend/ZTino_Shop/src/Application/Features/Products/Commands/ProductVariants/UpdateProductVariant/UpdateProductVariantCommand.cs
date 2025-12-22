using Application.Features.Products.DTOs.ProductVariants;

namespace Application.Features.Products.Commands.ProductVariants.UpdateProductVariant
{
    public record UpdateProductVariantCommand(UpsertProductVariantDto Dto) : IRequest<UpsertProductVariantDto>;
}
