namespace Application.Features.Auth.v1.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
