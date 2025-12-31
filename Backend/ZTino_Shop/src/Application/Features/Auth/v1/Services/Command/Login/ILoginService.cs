using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Command.Login
{
    public interface ILoginService
    {
        Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    }
}
