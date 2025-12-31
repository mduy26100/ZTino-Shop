namespace Application.Features.Auth.v1.DTOs
{
    public class JwtTokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
