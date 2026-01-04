namespace Application.Features.Carts.v1.Queries.GetCartById
{
    public class GetCartByIdValidator : AbstractValidator<GetCartByIdQuery>
    {
        public GetCartByIdValidator()
        {
            RuleFor(x => x.CartId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("CartId must be a valid GUID if provided.");
        }
    }
}
