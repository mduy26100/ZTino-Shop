namespace Infrastructure.Auth.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
