using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Command.Register
{
    public interface IRegisterService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);
    }
}
