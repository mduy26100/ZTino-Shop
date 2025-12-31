namespace Application.Features.Products.v1.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("Category data must be provided.");

            RuleFor(x => x.Dto.Id)
                .GreaterThan(0).WithMessage("Category Id must be greater than 0.");

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
