namespace Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageValidator : AbstractValidator<UpdateProductImageCommand>
    {
        public UpdateProductImageValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("Product image data must not be null.");

            When(x => x.Dto != null, () =>
            {
                RuleFor(x => x.Dto.Id)
                    .GreaterThan(0)
                    .WithMessage("ProductImage Id must be greater than 0.");

                RuleFor(x => x.Dto.ProductColorId)
                    .GreaterThan(0)
                    .WithMessage("ProductVariantId must be greater than 0.");

                RuleFor(x => x.Dto.ImageUrl)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Dto.ImageUrl))
                    .WithMessage("ImageUrl must not exceed 500 characters.");

                RuleFor(x => x.Dto.IsMain)
                    .NotNull()
                    .WithMessage("IsMain is required.");
            });
        }
    }
}
