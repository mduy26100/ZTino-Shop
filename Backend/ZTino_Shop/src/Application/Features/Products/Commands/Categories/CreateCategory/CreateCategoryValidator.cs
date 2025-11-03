namespace Application.Features.Products.Commands.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("Category data must be provided.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(200).WithMessage("Category name must not exceed 200 characters.");

            RuleFor(x => x.Dto.Slug)
                .NotEmpty().WithMessage("Category slug is required.")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens.")
                .MaximumLength(200).WithMessage("Slug must not exceed 200 characters.");

            RuleFor(x => x.Dto.ParentId)
                .GreaterThan(0).When(x => x.Dto.ParentId.HasValue)
                .WithMessage("ParentId must be greater than 0 if specified.");
        }
    }
}
