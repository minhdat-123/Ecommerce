using IdentityService.Application.Interfaces;
using IdentityService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, new[] { "Role name is required" });
            }

            if (await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' already exists" });
            }

            var result = await _roleRepository.CreateRoleAsync(roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to create role '{roleName}'" });
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, new[] { "Role name is required" });
            }

            if (!await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' does not exist" });
            }

            var result = await _roleRepository.DeleteRoleAsync(roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to delete role '{roleName}'" });
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return await _roleRepository.RoleExistsAsync(roleName);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<string>();
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new List<string>();
            }

            return await _userRepository.GetUserRolesAsync(user);
        }

        public async Task<(bool Succeeded, string[] Errors)> AddUserToRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return (false, new[] { "User ID is required" });
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, new[] { "Role name is required" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { $"User with ID '{userId}' not found" });
            }

            if (!await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' does not exist" });
            }

            var userRoles = await _userRepository.GetUserRolesAsync(user);
            if (userRoles.Contains(roleName))
            {
                return (false, new[] { $"User is already in role '{roleName}'" });
            }

            var result = await _roleRepository.AddUserToRoleAsync(user, roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to add user to role '{roleName}'" });
        }

        public async Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return (false, new[] { "User ID is required" });
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return (false, new[] { "Role name is required" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return (false, new[] { $"User with ID '{userId}' not found" });
            }

            if (!await _roleRepository.RoleExistsAsync(roleName))
            {
                return (false, new[] { $"Role '{roleName}' does not exist" });
            }

            var userRoles = await _userRepository.GetUserRolesAsync(user);
            if (!userRoles.Contains(roleName))
            {
                return (false, new[] { $"User is not in role '{roleName}'" });
            }

            var result = await _roleRepository.RemoveUserFromRoleAsync(user, roleName);
            return result ? 
                (true, Array.Empty<string>()) : 
                (false, new[] { $"Failed to remove user from role '{roleName}'" });
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var userRoles = await _userRepository.GetUserRolesAsync(user);
            return userRoles.Contains(roleName);
        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return new List<string>();
            }

            return await _roleRepository.GetUsersInRoleAsync(roleName);
        }
    }
}
