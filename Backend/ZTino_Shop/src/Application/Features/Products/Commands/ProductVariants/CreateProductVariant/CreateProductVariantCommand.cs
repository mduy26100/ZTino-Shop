using Application.Features.Products.DTOs.ProductVariants;

namespace Application.Features.Products.Commands.ProductVariants.CreateProductVariant
{
    public record CreateProductVariantCommand(UpsertProductVariantDto Dto) : IRequest<UpsertProductVariantDto>;
}
