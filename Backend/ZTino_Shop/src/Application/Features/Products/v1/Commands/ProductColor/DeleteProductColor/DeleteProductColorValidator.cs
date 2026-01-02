namespace Application.Features.Products.v1.Commands.ProductColor.DeleteProductColor
{
    public class DeleteProductColorValidator : AbstractValidator<DeleteProductColorCommand>
    {
        public DeleteProductColorValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product Color Id must be greater than 0.");
        }
    }
}
