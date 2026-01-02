using Domain.Enums;

namespace Application.Features.Auth.v1.DTOs
{
    public class LoginRequestDto
    {
        public required string Identifier { get; set; }
        public required string Password { get; set; }

        public LoginProvider Provider { get; set; } = LoginProvider.Credentials;

        public string? AccessToken { get; set; }
    }
}
