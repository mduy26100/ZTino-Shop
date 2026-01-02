namespace Application.Features.Products.v1.Commands.ProductColor.UpdateProductColor
{
    public class UpdateProductColorValidator : AbstractValidator<UpdateProductColorCommand>
    {
        public UpdateProductColorValidator()
        {
            RuleFor(x => x.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Product Color Id must be greater than 0.");

            RuleFor(x => x.Dto.ProductId)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than 0.");

            RuleFor(x => x.Dto.ColorId)
                .GreaterThan(0)
                .WithMessage("Color Id must be greater than 0.");
        }
    }
}
