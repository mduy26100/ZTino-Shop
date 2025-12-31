using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Command.UpdateProfile
{
    public interface IUpdateProfileService
    {
        Task<UserProfileDto> UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default);
    }
}
