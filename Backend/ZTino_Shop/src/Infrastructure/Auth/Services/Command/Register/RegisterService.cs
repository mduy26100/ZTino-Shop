using Application.Common.Exceptions;
using Application.Common.Interfaces.Identity;
using Application.Features.Auth.v1.DTOs;
using Application.Features.Auth.v1.Services.Command.Register;
using Domain.Consts;
using Infrastructure.Auth.Models;

namespace Infrastructure.Auth.Services.Command.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser _currentUserContext;

        public RegisterService(UserManager<ApplicationUser> userManager,
            ICurrentUser currentUserContext)
        {
            _userManager = userManager;
            _currentUserContext = currentUserContext;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
            {
                throw new ConflictException("UserName is already in use.");
            }

            var normalizedPhone = dto.PhoneNumber.Trim();

            var phoneExists = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken);

            if (phoneExists)
            {
                throw new ConflictException("Phone number is already in use.");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BusinessRuleException("Password confirmation does not match.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException($"Failed to create user: {errors}");
            }

            var currentRoles = _currentUserContext.Roles;
            var isManager = currentRoles.Contains(Roles.Manager);

            if (!isManager && !string.IsNullOrWhiteSpace(dto.Role))
            {
                throw new ForbiddenException("You are not allowed to assign roles.");
            }

            var roleToAssign = string.IsNullOrWhiteSpace(dto.Role)
                ? Roles.User
                : dto.Role;

            await _userManager.AddToRoleAsync(user, roleToAssign);

            return new RegisterResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName
            };
        }
    }
}
