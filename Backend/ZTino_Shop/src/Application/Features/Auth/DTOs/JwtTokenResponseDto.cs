namespace Application.Features.Auth.DTOs
{
    public class JwtTokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
