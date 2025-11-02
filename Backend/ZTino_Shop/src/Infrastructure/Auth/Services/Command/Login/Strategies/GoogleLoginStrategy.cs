using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Login.Strategy;
using Domain.Enums;

namespace Infrastructure.Auth.Services.Command.Login.Strategies
{
    public class GoogleLoginStrategy : ILoginStrategy
    {
        public LoginProvider Provider => LoginProvider.Google;

        public Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
