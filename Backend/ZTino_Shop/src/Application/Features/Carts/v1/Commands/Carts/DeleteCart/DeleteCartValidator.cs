namespace Application.Features.Carts.v1.Commands.Carts.DeleteCart
{
    public class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
    {
        public DeleteCartValidator()
        {
            RuleFor(x => x.CartItemId)
                .GreaterThan(0)
                .WithMessage("CartItemId must be greater than 0.");
        }
    }
}
