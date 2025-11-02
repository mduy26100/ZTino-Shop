namespace Application.Features.Auth.Services.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiration();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
