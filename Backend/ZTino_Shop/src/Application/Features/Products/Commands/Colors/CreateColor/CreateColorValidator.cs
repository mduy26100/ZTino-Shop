namespace Application.Features.Products.Commands.Colors.CreateColor
{
    public class CreateColorValidator : AbstractValidator<CreateColorCommand>
    {
        public CreateColorValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("Color data is required.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MaximumLength(50).WithMessage("Color name cannot exceed 50 characters.");
        }
    }
}
