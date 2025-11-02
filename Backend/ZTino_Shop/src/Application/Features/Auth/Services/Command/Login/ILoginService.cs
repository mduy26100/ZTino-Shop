using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Command.Login
{
    public interface ILoginService
    {
        Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    }
}
