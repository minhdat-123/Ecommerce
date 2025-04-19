using IdentityService.Application.Interfaces;
using IdentityService.Application.Models;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            var roles = await _userRepository.GetUserRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            var roles = await _userRepository.GetUserRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUsernameAsync(userName);
            if (user == null) return null;

            var roles = await _userRepository.GetUserRolesAsync(user);
            return MapToUserDto(user, roles);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            var result = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userRepository.GetUserRolesAsync(user);
                result.Add(MapToUserDto(user, roles));
            }

            return result;
        }

        public async Task<(bool Succeeded, string[] Errors)> RegisterUserAsync(RegisterUserDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            // Add user to default role
            await _userManager.AddToRoleAsync(user, "User");

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(string id, UserDto model)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string Token)> LoginAsync(LoginUserDto model)
        {
            // Check if the input is an email
            var user = await _userRepository.GetByEmailAsync(model.UserNameOrEmail);
            
            // If not found by email, try username
            if (user == null)
            {
                user = await _userRepository.GetByUsernameAsync(model.UserNameOrEmail);
            }

            if (user == null)
            {
                return (false, string.Empty);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            
            if (!result.Succeeded)
            {
                return (false, string.Empty);
            }

            // Note: In a real implementation, you would generate a JWT token here
            // For simplicity, we're returning a success message
            return (true, "Login successful");
        }

        public async Task<(bool Succeeded, string[] Errors)> AssignUserToRoleAsync(string userId, string roleName)
        {
            if (!await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' does not exist" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            var result = await _roleRepository.AddUserToRoleAsync(user, roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to add user to role '{roleName}'" });
        }

        public async Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            if (!await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' does not exist" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            var result = await _roleRepository.RemoveUserFromRoleAsync(user, roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to remove user from role '{roleName}'" });
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userRepository.GetUserRolesAsync(user);
        }

        public async Task<(bool Succeeded, string[] Errors)> LockUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            // Set lockout end date to 100 years from now (effectively permanent until unlocked)
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string[] Errors)> UnlockUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            // Remove lockout end date
            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        public async Task<bool> IsLockedOutAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<(bool Succeeded, string[] Errors)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string[] Errors)> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { "User not found" });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, Array.Empty<string>());
        }

        private UserDto MapToUserDto(ApplicationUser user, IEnumerable<string> roles)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                Roles = roles.ToList()
            };
        }
    }
}
