using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(RegisterRequestDto Dto) : IRequest<RegisterResponseDto>;
}
