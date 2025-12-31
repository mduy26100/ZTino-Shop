namespace Application.Features.Auth.v1.Commands.UpdateProfile
{
    internal class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileValidator()
        {

            RuleFor(x => x.Dto.FirstName)
                .MaximumLength(50).WithMessage("First name must be at most 50 characters.");

            RuleFor(x => x.Dto.LastName)
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters.");

            RuleFor(x => x.Dto.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number must be at most 20 characters.");

            RuleFor(x => x.Dto.AvatarUrl)
                .MaximumLength(500).WithMessage("Avatar URL must be at most 500 characters.");

            RuleFor(x => x.Dto.Role)
                .MaximumLength(50).WithMessage("Role must be at most 50 characters.");
        }
    }
}
