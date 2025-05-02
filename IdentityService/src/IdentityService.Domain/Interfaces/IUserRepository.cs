using IdentityService.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IdentityService.Domain.Interfaces
{
    /// <summary>
    /// Interface for user-related repository operations
    /// </summary>
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByUsernameAsync(string username);
        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> CreateAsync(ApplicationUser user, string password);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(string id);
    }
}
