namespace Application.Features.Auth.v1.DTOs
{
    public class RegisterResponseDto
    {
        public Guid UserId { get; set; }
        public required string UserName { get; set; }
    }
}
