namespace Application.Features.Products.v1.Queries.Products.GetProductDetailBySlug
{
    public class GetProductDetailBySlugValidator : AbstractValidator<GetProductDetailBySlugQuery>
    {
        public GetProductDetailBySlugValidator()
        {
            RuleFor(x => x.slug)
                .NotEmpty().WithMessage("Product slug is required.")
                .MinimumLength(3).WithMessage("Slug is too short; please check again.")
                .MaximumLength(150).WithMessage("Slug must not exceed 150 characters.")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug can only contain lowercase letters, numbers, and hyphens.");
        }
    }
}
