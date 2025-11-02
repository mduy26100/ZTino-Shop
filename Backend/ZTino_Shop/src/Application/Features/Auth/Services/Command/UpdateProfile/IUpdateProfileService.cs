using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Command.UpdateProfile
{
    public interface IUpdateProfileService
    {
        Task<UserProfileDto> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default);
    }
}
