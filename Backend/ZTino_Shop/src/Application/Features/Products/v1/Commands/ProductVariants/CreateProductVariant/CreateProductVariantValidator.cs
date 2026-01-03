namespace Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant
{
    public class CreateProductVariantValidator : AbstractValidator<CreateProductVariantCommand>
    {
        public CreateProductVariantValidator()
        {
            RuleFor(v => v.Dto.ProductColorId)
                .GreaterThan(0)
                .WithMessage("Product color id must be greater than 0.");

            RuleFor(v => v.Dto.SizeId)
                .GreaterThan(0)
                .WithMessage("Size id must be greater than 0.");

            RuleFor(v => v.Dto.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity must be greater than or equal to 0.");

            RuleFor(v => v.Dto.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
        }
    }
}
