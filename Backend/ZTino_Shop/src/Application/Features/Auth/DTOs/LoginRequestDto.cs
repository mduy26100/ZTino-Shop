using Domain.Enums;

namespace Application.Features.Auth.DTOs
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public LoginProvider Provider { get; set; } = LoginProvider.EmailPassword;

        public string? AccessToken { get; set; }
    }
}
