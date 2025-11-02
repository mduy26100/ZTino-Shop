using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Login.Strategy;
using Domain.Enums;

namespace Infrastructure.Auth.Services.Command.Login.Strategies
{
    public class FacebookLoginStrategy : ILoginStrategy
    {
        public LoginProvider Provider => LoginProvider.Facebook;

        public Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
