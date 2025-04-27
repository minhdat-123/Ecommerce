using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Blazor.Services
{
    /// <summary>
    /// Interface for identity-related operations via the API gateway
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Gets the current user's profile information
        /// </summary>
        Task<UserProfileDto> GetUserProfileAsync();
        
        /// <summary>
        /// Gets a user by ID (admin only)
        /// </summary>
        Task<UserProfileDto> GetUserByIdAsync(string userId);
        
        /// <summary>
        /// Gets all users (admin only)
        /// </summary>
        Task<IEnumerable<UserProfileDto>> GetAllUsersAsync();
        
        /// <summary>
        /// Gets a user's roles
        /// </summary>
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        
        /// <summary>
        /// Updates a user's profile
        /// </summary>
        Task<bool> UpdateUserProfileAsync(UserProfileDto user);
    }
    
    /// <summary>
    /// Data transfer object for user profile information
    /// </summary>
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
