namespace Application.Common.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task<bool> UserExistsAsync(Guid userId);
        Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId);
    }
}
