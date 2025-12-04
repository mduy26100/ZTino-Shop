namespace Application.Features.Products.Commands.Colors.DeleteColor
{
    public class DeleteColorValidator : AbstractValidator<DeleteColorCommand>
    {
        public DeleteColorValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Color Id must be greater than 0.");
        }
    }
}
