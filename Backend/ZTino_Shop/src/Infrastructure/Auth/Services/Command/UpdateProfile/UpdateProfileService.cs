using Application.Common.Interfaces.Shared;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.UpdateProfile;
using Application.Features.Auth.Services.Command.UpdateProfile.Factory;
using Application.Features.Auth.Services.Command.UpdateProfile.Strategy;
using Domain.Consts;

namespace Infrastructure.Auth.Services.Command.UpdateProfile
{
    public class UpdateProfileService : IUpdateProfileService
    {
        private readonly IUpdateProfileStrategyFactory _strategyFactory;
        private readonly ICurrentUserContext _currentUserContext;

        public UpdateProfileService(
            IUpdateProfileStrategyFactory strategyFactory,
            ICurrentUserContext currentUserContext)
        {
            _strategyFactory = strategyFactory;
            _currentUserContext = currentUserContext;
        }

        public async Task<UserProfileDto> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default)
        {
            var currentUserId = _currentUserContext.UserId;
            var currentUserRoles = _currentUserContext.Roles;

            if (!currentUserRoles.Contains(Roles.Manager) && currentUserId != dto.Id)
                throw new UnauthorizedAccessException("You can only update your own profile.");

            IUpdateProfileStrategy strategy = currentUserId == dto.Id
                ? _strategyFactory.GetSelfStrategy()
                : _strategyFactory.GetManagerStrategy();

            var updatedUser = await strategy.UpdateAsync(dto, cancellationToken);
            return updatedUser;
        }
    }
}