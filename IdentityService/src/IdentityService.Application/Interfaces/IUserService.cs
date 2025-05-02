using IdentityService.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Application.Interfaces
{
    /// <summary>
    /// Interface for user management operations in the application layer
    /// </summary>
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<(bool Succeeded, string[] Errors)> RegisterUserAsync(RegisterUserDto model);
        Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(string id, UserDto model);
        Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string id);
        Task<(bool Succeeded, string Token)> LoginAsync(LoginUserDto model);
        Task<(bool Succeeded, string[] Errors)> AssignUserToRoleAsync(string userId, string roleName);
        Task<(bool Succeeded, string[] Errors)> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        
        // User locking and account management
        Task<(bool Succeeded, string[] Errors)> LockUserAsync(string userId);
        Task<(bool Succeeded, string[] Errors)> UnlockUserAsync(string userId);
        Task<bool> IsLockedOutAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<(bool Succeeded, string[] Errors)> ConfirmEmailAsync(string userId, string token);
        Task<(bool Succeeded, string[] Errors)> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}
