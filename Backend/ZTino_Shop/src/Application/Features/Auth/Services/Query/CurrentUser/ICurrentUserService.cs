using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Services.Query.CurrentUser
{
    public interface ICurrentUserService
    {
        Task<UserProfileDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
