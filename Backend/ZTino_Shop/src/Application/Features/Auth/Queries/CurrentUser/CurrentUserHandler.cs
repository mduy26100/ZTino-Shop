using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Shared;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Query.CurrentUser;

namespace Application.Features.Auth.Queries.CurrentUser
{
    public class CurrentUserHandler : IRequestHandler<CurrentUserQuery, UserProfileDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ILoggingService<CurrentUserHandler> _logger;

        public CurrentUserHandler(
            ICurrentUserService currentUserService,
            ICurrentUserContext currentUserContext,
            ILoggingService<CurrentUserHandler> logger)
        {
            _currentUserService = currentUserService;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        public async Task<UserProfileDto> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserContext.UserId;
            _logger.LogInformation($"[CurrentUserHandler] Fetching current user info for UserId: {userId}");

            try
            {
                var result = await _currentUserService.GetCurrentUserAsync(userId, cancellationToken);
                _logger.LogInformation($"[CurrentUserHandler] Successfully fetched info for UserId: {userId}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CurrentUserHandler] Failed to fetch current user info for UserId: {userId}", ex);
                throw;
            }
        }
    }
}
