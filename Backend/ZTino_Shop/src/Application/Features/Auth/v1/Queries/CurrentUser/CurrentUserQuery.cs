using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Queries.CurrentUser
{
    public record CurrentUserQuery() : IRequest<UserProfileDto>;
}
