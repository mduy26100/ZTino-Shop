using Application.Features.Auth.v1.DTOs;

namespace Application.Features.Auth.v1.Commands.UpdateProfile
{
    public record UpdateProfileCommand(UpdateProfileDto Dto) : IRequest<UserProfileDto>;
}
