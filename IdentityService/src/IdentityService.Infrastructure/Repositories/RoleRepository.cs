using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRepository(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await RoleExistsAsync(roleName))
            {
                return false;
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }

        public async Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return true; // User is already in role
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                return false;
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                return true; // User is not in role, so no need to remove
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetUsersInRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return new List<string>();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            return usersInRole.Select(u => u.Id).ToList();
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user)
        {
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }
    }
}
