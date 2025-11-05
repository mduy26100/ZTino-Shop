namespace Application.Features.Products.Commands.Products.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("Product data is required.");

            RuleFor(x => x.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than 0.");

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
        }
    }
}
