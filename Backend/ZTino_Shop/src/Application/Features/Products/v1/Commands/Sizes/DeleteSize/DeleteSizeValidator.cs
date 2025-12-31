namespace Application.Features.Products.v1.Commands.Sizes.DeleteSize
{
    public class DeleteSizeValidator : AbstractValidator<DeleteSizeCommand>
    {
        public DeleteSizeValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0)
                .WithMessage("Size id must be greater than 0.");
        }
    }
}
