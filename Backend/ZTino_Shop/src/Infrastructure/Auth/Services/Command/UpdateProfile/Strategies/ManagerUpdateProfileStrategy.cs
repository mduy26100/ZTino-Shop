using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.UpdateProfile.Strategy;
using Infrastructure.Auth.Models;

namespace Infrastructure.Auth.Services.Command.UpdateProfile.Strategies
{
    public class ManagerUpdateProfileStrategy : IUpdateProfileStrategy
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IFileUploadService _fileUploadService;

        public ManagerUpdateProfileStrategy(UserManager<ApplicationUser> userManager,
                                           RoleManager<ApplicationRole> roleManager,
                                           IFileUploadService fileUploadService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _fileUploadService = fileUploadService;
        }

        public async Task<UserProfileDto> UpdateAsync(UpdateProfileDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (dto.AvatarImageContent != null && !string.IsNullOrWhiteSpace(dto.AvatarImageFileName))
            {
                var uploadRequest = new FileUploadRequest
                {
                    Content = dto.AvatarImageContent,
                    FileName = dto.AvatarImageFileName!,
                    ContentType = dto.AvatarImageContentType ?? "image/jpeg"
                };
                user.AvatarUrl = await _fileUploadService.UploadAsync(uploadRequest, cancellationToken);
            }

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to update user.");

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                if (!await _roleManager.RoleExistsAsync(dto.Role))
                    throw new InvalidOperationException("Role not found");

                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            var updatedRoles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = $"{user.FirstName} {user.LastName}",
                AvatarUrl = user.AvatarUrl,
                Roles = updatedRoles.ToList()
            };
        }
    }
}