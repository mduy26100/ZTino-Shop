namespace Application.Features.Products.Commands.ProductImages.CreateProductImage
{
    public class CreateProductImagesValidator : AbstractValidator<CreateProductImagesCommand>
    {
        public CreateProductImagesValidator()
        {
            RuleFor(x => x.Dtos)
                .NotEmpty()
                .WithMessage("Product images list must not be empty.");

            RuleForEach(x => x.Dtos).ChildRules(dto =>
            {
                dto.RuleFor(x => x.ProductVariantId)
                    .GreaterThan(0)
                    .WithMessage("ProductVariantId must be greater than 0.");

                dto.RuleFor(x => x.ImageUrl)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
                    .WithMessage("ImageUrl must not exceed 500 characters.");
            });
        }
    }
}
