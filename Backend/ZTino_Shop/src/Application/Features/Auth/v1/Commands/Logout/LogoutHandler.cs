using Application.Common.Interfaces.Identity;
using Application.Features.Auth.v1.Services.Command.Logout;

namespace Application.Features.Auth.v1.Commands.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly ILogoutService _logoutService;
        private readonly ICurrentUser _currentUserContext;

        public LogoutHandler(
            ILogoutService logoutService,
            ICurrentUser currentUserContext)
        {
            _logoutService = logoutService;
            _currentUserContext = currentUserContext;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserContext.UserId;

            var result = await _logoutService.LogoutAsync(userId, cancellationToken);
            return result;
        }
    }
}