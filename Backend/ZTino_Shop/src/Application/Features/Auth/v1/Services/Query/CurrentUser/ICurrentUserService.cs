using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Services.Query.CurrentUser
{
    public interface ICurrentUserService
    {
        Task<UserProfileDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
