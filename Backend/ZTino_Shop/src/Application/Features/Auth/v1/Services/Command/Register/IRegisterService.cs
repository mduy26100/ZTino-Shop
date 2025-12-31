using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Command.Register
{
    public interface IRegisterService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);
    }
}
