using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Commands.Login
{
    public record LoginCommand(LoginRequestDto Dto) : IRequest<JwtTokenResponseDto>;
}
