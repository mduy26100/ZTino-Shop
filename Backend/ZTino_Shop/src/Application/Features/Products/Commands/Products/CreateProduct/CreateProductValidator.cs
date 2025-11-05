namespace Application.Features.Products.Commands.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Dto.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

            RuleFor(x => x.Dto.BasePrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Base price must be non-negative.");

            RuleFor(x => x.Dto.MainImageUrl)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Dto.MainImageUrl))
                .WithMessage("MainImageUrl must not exceed 500 characters.");
        }
    }
}
