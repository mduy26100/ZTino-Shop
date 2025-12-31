using Application.Features.Products.v1.DTOs.ProductVariants;

namespace Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant
{
    public record CreateProductVariantCommand(UpsertProductVariantDto Dto) : IRequest<UpsertProductVariantDto>;
}
