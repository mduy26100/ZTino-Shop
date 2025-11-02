using Application.Common.Interfaces.Services.User;
using Infrastructure.Auth.Models;

namespace Infrastructure.Common.Interfaces.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Array.Empty<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList().AsReadOnly();
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user != null;
        }
    }
}
