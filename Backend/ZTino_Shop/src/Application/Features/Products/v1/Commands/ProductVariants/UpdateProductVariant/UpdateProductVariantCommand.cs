using Application.Features.Products.v1.DTOs.ProductVariants;

namespace Application.Features.Products.v1.Commands.ProductVariants.UpdateProductVariant
{
    public record UpdateProductVariantCommand(UpsertProductVariantDto Dto) : IRequest<UpsertProductVariantDto>;
}
