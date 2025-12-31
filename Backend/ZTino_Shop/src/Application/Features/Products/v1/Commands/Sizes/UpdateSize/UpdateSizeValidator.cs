namespace Application.Features.Products.v1.Commands.Sizes.UpdateSize
{
    public class UpdateSizeValidator : AbstractValidator<UpdateSizeCommand>
    {
        public UpdateSizeValidator()
        {
            RuleFor(v => v.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Size id must be greater than 0.");

            RuleFor(v => v.Dto.Name)
                .NotEmpty().WithMessage("Size name is required.")
                .MaximumLength(50).WithMessage("Size name must not exceed 50 characters.")
                .MinimumLength(1).WithMessage("Size name must be at least 1 character long.");
        }
    }
}
