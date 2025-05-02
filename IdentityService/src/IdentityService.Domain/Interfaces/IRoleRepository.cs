using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    /// <summary>
    /// Interface for role-related repository operations
    /// </summary>
    public interface IRoleRepository
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName);
        Task<IEnumerable<string>> GetUsersInRoleAsync(string roleName);
    }
}
