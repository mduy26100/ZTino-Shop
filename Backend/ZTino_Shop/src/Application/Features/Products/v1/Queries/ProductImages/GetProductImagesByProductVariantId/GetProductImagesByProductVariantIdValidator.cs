namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductVariantId;

public class GetProductImagesByProductVariantIdValidator : AbstractValidator<GetProductImagesByProductVariantIdQuery>
{
    public GetProductImagesByProductVariantIdValidator()
    {
        RuleFor(x => x.variantId)
                .GreaterThan(0)
                .WithMessage("Product Variant Id must be greater than 0.");
    }
}