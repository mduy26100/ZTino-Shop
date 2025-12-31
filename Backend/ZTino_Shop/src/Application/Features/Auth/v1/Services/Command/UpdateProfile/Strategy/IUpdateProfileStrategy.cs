using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Command.UpdateProfile.Strategy
{
    public interface IUpdateProfileStrategy
    {
        Task<UserProfileDto> UpdateAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default);
    }
}
