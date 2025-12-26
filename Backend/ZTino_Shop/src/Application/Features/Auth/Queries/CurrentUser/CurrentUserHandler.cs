using Application.Common.Interfaces.Identity;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Query.CurrentUser;

namespace Application.Features.Auth.Queries.CurrentUser
{
    public class CurrentUserHandler : IRequestHandler<CurrentUserQuery, UserProfileDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentUser _currentUserContext;

        public CurrentUserHandler(
            ICurrentUserService currentUserService,
            ICurrentUser currentUserContext)
        {
            _currentUserService = currentUserService;
            _currentUserContext = currentUserContext;
        }

        public async Task<UserProfileDto> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserContext.UserId;

            var result = await _currentUserService.GetCurrentUserAsync(userId, cancellationToken);
            return result;
        }
    }
}
