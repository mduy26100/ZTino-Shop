using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Commands.Register
{
    public record RegisterCommand(RegisterRequestDto Dto) : IRequest<RegisterResponseDto>;
}
