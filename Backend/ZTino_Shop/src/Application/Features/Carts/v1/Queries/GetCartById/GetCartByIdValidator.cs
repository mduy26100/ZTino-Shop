namespace Application.Features.Carts.v1.Queries.GetCartById
{
    public class GetCartByIdValidator : AbstractValidator<GetCartByIdQuery>
    {
        public GetCartByIdValidator()
        {
            RuleFor(x => x.CartId)
                .NotEmpty()
                .WithMessage("CartId is required.");
        }
    }
}
