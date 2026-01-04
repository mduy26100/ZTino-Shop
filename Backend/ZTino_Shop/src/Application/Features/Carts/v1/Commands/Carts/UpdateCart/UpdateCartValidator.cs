namespace Application.Features.Carts.v1.Commands.Carts.UpdateCart
{
    public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartValidator()
        {
            RuleFor(x => x.Dto.CartId)
                .NotEmpty()
                .WithMessage("CartId is required for update operation.");

            RuleFor(x => x.Dto.ProductVariantId)
                .GreaterThan(0)
                .WithMessage("ProductVariantId must be greater than 0.");

            RuleFor(x => x.Dto.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be greater than or equal to 0. Set to 0 to remove item from cart.");
        }
    }
}
