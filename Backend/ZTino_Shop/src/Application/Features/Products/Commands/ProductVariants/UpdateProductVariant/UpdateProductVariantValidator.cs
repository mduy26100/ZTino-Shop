namespace Application.Features.Products.Commands.ProductVariants.UpdateProductVariant
{
    public class UpdateProductVariantValidator : AbstractValidator<UpdateProductVariantCommand>
    {
        public UpdateProductVariantValidator()
        {
            RuleFor(v => v.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Product variant id must be greater than 0.");

            RuleFor(v => v.Dto.ProductId)
                .GreaterThan(0)
                .WithMessage("Product id must be greater than 0.");

            RuleFor(v => v.Dto.ColorId)
                .GreaterThan(0)
                .WithMessage("Color id must be greater than 0.");

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
