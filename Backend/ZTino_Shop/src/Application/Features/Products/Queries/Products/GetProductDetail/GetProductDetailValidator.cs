namespace Application.Features.Products.Queries.Products.GetProductDetail
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
