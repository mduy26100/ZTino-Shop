namespace Application.Features.Auth.v1.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Identifier)
                .NotEmpty().WithMessage("Identifier is required.")
                .Must(id => id.Contains("@") ? IsValidEmail(id) : IsValidUserName(id))
                .WithMessage("Identifier must be a valid email or username.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }

        private bool IsValidEmail(string email)
        {
            var emailAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        private bool IsValidUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (username.Length < 3 || username.Length > 50) return false;
            return username.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.');
        }
    }
}
