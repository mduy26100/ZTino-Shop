using Application.Common.Interfaces.Shared;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Services.Command.Register;
using Domain.Consts;
using Infrastructure.Auth.Models;

namespace Infrastructure.Auth.Services.Command.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserContext _currentUserContext;

        public RegisterService(UserManager<ApplicationUser> userManager,
            ICurrentUserContext currentUserContext)
        {
            _userManager = userManager;
            _currentUserContext = currentUserContext;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
        {
            if(await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            if(dto.Password != dto.ConfirmPassword)
            {
                throw new InvalidOperationException("Password confirmation does not match.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            var currentRoles = _currentUserContext.Roles;
            var isManager = currentRoles.Contains(Roles.Manager);

            var roleToAssign = isManager && !string.IsNullOrWhiteSpace(dto.Role)
                ? dto.Role
                : Roles.User;

            await _userManager.AddToRoleAsync(user, roleToAssign);

            return new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                Role = roleToAssign,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
