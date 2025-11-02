using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Login;
using Application.Features.Auth.Services.Command.Login.Factory;

namespace Infrastructure.Auth.Services.Command.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginStrategyFactory _strategyFactory;

        public LoginService(ILoginStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
        }

        public Task<JwtTokenResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
        {
            var strategy = _strategyFactory.GetStrategy(dto.Provider);
            return strategy.LoginAsync(dto, cancellationToken);
        }
    }
}
