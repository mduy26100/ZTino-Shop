using Application.Features.Auth.v1.DTOs;
using Domain.Enums;

namespace Application.Features.Auth.v1.Services.Command.Login.Strategy
{
    public interface ILoginStrategy
    {
        LoginProvider Provider { get; }
        Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken);
    }
}
