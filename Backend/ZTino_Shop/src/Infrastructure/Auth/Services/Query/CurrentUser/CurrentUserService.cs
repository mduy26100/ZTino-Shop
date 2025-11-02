using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Query.CurrentUser;
using Infrastructure.Auth.Models;

namespace Infrastructure.Auth.Services.Query.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FirstName + " " + user.LastName,
                AvatarUrl = user.AvatarUrl,
                Roles = roles.ToList()
            };
        }
    }
}
