using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Identity;
using Application.Features.Auth.Services.Command.Logout;

namespace Application.Features.Auth.Commands.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly ILogoutService _logoutService;
        private readonly ICurrentUser _currentUserContext;
        private readonly ILoggingService<LogoutHandler> _logger;

        public LogoutHandler(
            ILogoutService logoutService,
            ICurrentUser currentUserContext,
            ILoggingService<LogoutHandler> logger)
        {
            _logoutService = logoutService;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserContext.UserId;
            _logger.LogInformation($"[LogoutHandler] Attempting logout for userId: {userId}");

            try
            {
                var result = await _logoutService.LogoutAsync(userId, cancellationToken);
                _logger.LogInformation($"[LogoutHandler] Logout {(result ? "successful" : "no active tokens")} for userId: {userId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[LogoutHandler] Logout failed for userId: {userId}", ex);
                throw;
            }
        }
    }
}