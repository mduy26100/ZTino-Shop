namespace Application.Features.Products.Commands.Sizes.CreateSize
{
    public class CreateSizeValidator : AbstractValidator<CreateSizeCommand>
    {
        public CreateSizeValidator()
        {
            RuleFor(v => v.Dto.Name)
                .NotEmpty().WithMessage("Size name is required.")
                .MaximumLength(50).WithMessage("Size name must not exceed 50 characters.")
                .MinimumLength(1).WithMessage("Size name must be at least 1 character long.");
        }
    }
}
