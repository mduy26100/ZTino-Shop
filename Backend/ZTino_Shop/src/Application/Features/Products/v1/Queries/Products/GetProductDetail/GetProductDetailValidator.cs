namespace Application.Features.Products.v1.Queries.Products.GetProductDetail
{
    public class GetProductDetailValidator : AbstractValidator<GetProductDetailQuery>
    {
        public GetProductDetailValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");
        }
    }
}
