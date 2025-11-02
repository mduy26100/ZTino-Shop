using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.Login
{
    public record LoginCommand(LoginRequestDto Dto) : IRequest<JwtTokenResponseDto>;
}
