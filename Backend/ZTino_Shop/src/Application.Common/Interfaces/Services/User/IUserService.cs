namespace Application.Common.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(Guid userId);
        Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId);
    }
}
