using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Command.TokenRefresh
{
    public interface ITokenRefreshService
    {
        Task<JwtTokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
