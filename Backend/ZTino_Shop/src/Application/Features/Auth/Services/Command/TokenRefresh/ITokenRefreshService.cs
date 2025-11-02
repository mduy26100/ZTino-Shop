using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Command.TokenRefresh
{
    public interface ITokenRefreshService
    {
        Task<JwtTokenResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
