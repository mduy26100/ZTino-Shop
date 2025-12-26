using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Login;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, JwtTokenResponseDto>
    {
        private readonly ILoginService _loginService;

        public LoginHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<JwtTokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _loginService.LoginAsync(request.Dto, cancellationToken);
            return result;
        }
    }
}
