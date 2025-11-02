using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Shared;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.UpdateProfile;

namespace Application.Features.Auth.Commands.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
    {
        private readonly IUpdateProfileService _updateProfileService;
        private readonly ILoggingService<UpdateProfileHandler> _logger;
        private readonly ICurrentUserContext _currentUserContext;


        public UpdateProfileHandler(
            IUpdateProfileService updateProfileService,
            ILoggingService<UpdateProfileHandler> logger,
            ICurrentUserContext currentUserContext)
        {
            _updateProfileService = updateProfileService;
            _logger = logger;
            _currentUserContext = currentUserContext;
        }

        public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[UpdateProfile] Update profile attempt for userId: {request.Dto.Id}");

            try
            {
                if (request.Dto.Id == Guid.Empty)
                {
                    request.Dto.Id = _currentUserContext.UserId;
                }

                var result = await _updateProfileService.UpdateProfileAsync(request.Dto, cancellationToken);
                _logger.LogInformation($"[UpdateProfile] Update profile successful for userId: {request.Dto.Id}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateProfile] Update profile failed for userId: {request.Dto.Id}", ex);
                throw;
            }
        }
    }
}
