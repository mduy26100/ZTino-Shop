namespace Application.Features.Auth.DTOs
{
    public class UpdateProfileDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }

        public Stream? AvatarImageContent { get; set; }
        public string? AvatarImageFileName { get; set; }
        public string? AvatarImageContentType { get; set; }
    }
}
