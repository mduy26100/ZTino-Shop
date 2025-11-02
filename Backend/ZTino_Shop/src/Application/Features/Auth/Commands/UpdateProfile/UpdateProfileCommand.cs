using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.UpdateProfile
{
    public record UpdateProfileCommand(UpdateProfileDto Dto) : IRequest<UserProfileDto>;
}
