namespace Application.Features.Products.v1.Commands.Colors.UpdateColor
{
    public class UpdateColorValidator : AbstractValidator<UpdateColorCommand>
    {
        public UpdateColorValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("Color data is required.");

            RuleFor(x => x.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Color ID must be greater than 0.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .WithMessage("Color name is required.")
                .MaximumLength(50)
                .WithMessage("Color name cannot exceed 50 characters.");
        }
    }
}
