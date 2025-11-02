using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Command.UpdateProfile.Strategy
{
    public interface IUpdateProfileStrategy
    {
        Task<UserProfileDto> UpdateAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default);
    }
}
