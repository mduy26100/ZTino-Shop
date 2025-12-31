using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Commands.TokenRefresh
{
    public record TokenRefreshCommand(string refreshToken) : IRequest<JwtTokenResponseDto>;
}
