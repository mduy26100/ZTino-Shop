namespace Application.Features.Products.v1.Commands.ProductVariants.DeleteProductVariant
{
    public record DeleteProductVariantCommand(int Id) : IRequest;
}
