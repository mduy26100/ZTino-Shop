namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductColorId
{
    public class GetProductImagesByProductColorIdValidator : AbstractValidator<GetProductImagesByProductColorIdQuery>
    {
        public GetProductImagesByProductColorIdValidator()
        {
            RuleFor(x => x.productColorId)
                    .GreaterThan(0)
                    .WithMessage("Product Variant Id must be greater than 0.");
        }
    }
}