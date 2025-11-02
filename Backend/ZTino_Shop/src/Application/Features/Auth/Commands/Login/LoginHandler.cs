using Application.Common.Interfaces.Logging;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Login;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, JwtTokenResponseDto>
    {
        private readonly ILoginService _loginService;
        private readonly ILoggingService<LoginHandler> _logger;

        public LoginHandler(ILoginService loginService, ILoggingService<LoginHandler> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        public async Task<JwtTokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[LoginHandler] Attempting login for {request.Dto.Email}");

            try
            {
                var result = await _loginService.LoginAsync(request.Dto, cancellationToken);
                _logger.LogInformation($"[LoginHandler] Login successful for {request.Dto.Email}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[LoginHandler] Login failed for {request.Dto.Email}", ex);
                throw;
            }
        }
    }
}
