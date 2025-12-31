using Application.Features.Auth.v1.DTOs;

namespace WebAPI.Requests.Auth
{
    public class UpdateProfileForm
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public IFormFile? AvatarImage { get; set; }

        public UpdateProfileDto ToDto()
        {
            return new UpdateProfileDto
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Role = Role,
                AvatarImageContent = AvatarImage?.OpenReadStream(),
                AvatarImageFileName = AvatarImage?.FileName,
                AvatarImageContentType = AvatarImage?.ContentType
            };
        }
    }
}
