namespace Application.Features.Auth.v1.Services.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        DateTime GetRefreshTokenExpiration();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
