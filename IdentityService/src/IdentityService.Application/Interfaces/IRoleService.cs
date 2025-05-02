using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Application.Interfaces
{
    /// <summary>
    /// Interface for role management operations in the application layer
    /// </summary>
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(string roleName);
        Task<(bool Succeeded, string[] Errors)> DeleteRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        
        // User-role management methods
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<(bool Succeeded, string[] Errors)> AddUserToRoleAsync(string userId, string roleName);
        Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUsersInRoleAsync(string roleName);
    }
}
