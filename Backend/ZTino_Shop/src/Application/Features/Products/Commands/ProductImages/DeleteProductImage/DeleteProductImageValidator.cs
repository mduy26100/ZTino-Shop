namespace Application.Features.Products.Commands.ProductImages.DeleteProductImage
{
    public class DeleteProductImageValidator : AbstractValidator<DeleteProductImageCommand>
    {
        public DeleteProductImageValidator()
        {
            RuleFor(x => x.Id)
               .GreaterThan(0)
               .WithMessage("Product Image Id must be greater than 0.");
        }
    }
}
