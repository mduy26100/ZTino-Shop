namespace Application.Features.Products.Commands.ProductVariants.DeleteProductVariant
{
    internal class DeleteProductVariantValidator : AbstractValidator<DeleteProductVariantCommand>
    {
        public DeleteProductVariantValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0)
                .WithMessage("Product variant id must be greater than 0.");
        }
    }
}
