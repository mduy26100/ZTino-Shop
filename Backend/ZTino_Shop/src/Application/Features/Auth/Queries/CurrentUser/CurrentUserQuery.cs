using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Queries.CurrentUser
{
    public record CurrentUserQuery() : IRequest<UserProfileDto>;
}
