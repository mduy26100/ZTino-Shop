namespace Application.Features.Products.v1.Queries.ProductColors.GetColorsByProductId
{
    public class GetColorsByProductIdValidator : AbstractValidator<GetColorsByProductIdQuery>
    {
        public GetColorsByProductIdValidator()
        {
            RuleFor(x => x.productId)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than 0.");
        }
    }
}
