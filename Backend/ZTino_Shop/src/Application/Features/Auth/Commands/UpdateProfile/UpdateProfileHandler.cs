using Application.Common.Interfaces.Identity;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.UpdateProfile;

namespace Application.Features.Auth.Commands.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
    {
        private readonly IUpdateProfileService _updateProfileService;
        private readonly ICurrentUser _currentUserContext;


        public UpdateProfileHandler(
            IUpdateProfileService updateProfileService,
            ICurrentUser currentUserContext)
        {
            _updateProfileService = updateProfileService;
            _currentUserContext = currentUserContext;
        }

        public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (request.Dto.Id == Guid.Empty)
            {
                request.Dto.Id = _currentUserContext.UserId;
            }

            var result = await _updateProfileService.UpdateProfileAsync(request.Dto, cancellationToken);

            return result;
        }
    }
}
