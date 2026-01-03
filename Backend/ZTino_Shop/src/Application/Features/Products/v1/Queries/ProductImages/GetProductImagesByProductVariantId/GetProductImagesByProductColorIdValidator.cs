namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductVariantId
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