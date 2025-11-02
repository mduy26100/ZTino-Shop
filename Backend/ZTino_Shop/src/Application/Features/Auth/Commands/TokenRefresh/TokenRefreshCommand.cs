using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.TokenRefresh
{
    public record TokenRefreshCommand(string refreshToken) : IRequest<JwtTokenResponseDto>;
}
