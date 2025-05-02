using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Models;
using IdentityService.API.Areas.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace IdentityService.API.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class UserManagementModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IdentityUIService _identityUIService;
        private readonly ILogger<UserManagementModel> _logger;

        public UserManagementModel(
            IUserService userService,
            IRoleService roleService,
            IdentityUIService identityUIService,
            ILogger<UserManagementModel> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _identityUIService = identityUIService;
            _logger = logger;
        }

        public IList<UserDto> Users { get; set; }
        
        [BindProperty]
        public string SelectedUserId { get; set; }
        
        [BindProperty]
        public string RoleToAdd { get; set; }
        
        public SelectList AvailableRoles { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                Users = users.ToList();

                var roles = await _roleService.GetAllRolesAsync();
                AvailableRoles = new SelectList(roles);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user management page");
                StatusMessage = "Error: Failed to load users. Please try again later.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddRoleAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId) || string.IsNullOrEmpty(RoleToAdd))
            {
                StatusMessage = "Error: User ID and Role must be selected.";
                return RedirectToPage();
            }

            try
            {
                var result = await _roleService.AddUserToRoleAsync(SelectedUserId, RoleToAdd);
                if (result.Succeeded)
                {
                    StatusMessage = $"Successfully added role '{RoleToAdd}' to user.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to add role. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding role {Role} to user {UserId}", RoleToAdd, SelectedUserId);
                StatusMessage = "Error: An unexpected error occurred while adding the role.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                StatusMessage = "Error: User ID and Role are required.";
                return RedirectToPage();
            }

            try
            {
                var result = await _roleService.RemoveUserFromRoleAsync(userId, role);
                if (result.Succeeded)
                {
                    StatusMessage = $"Successfully removed role '{role}' from user.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to remove role. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role {Role} from user {UserId}", role, userId);
                StatusMessage = "Error: An unexpected error occurred while removing the role.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLockUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Error: User ID is required.";
                return RedirectToPage();
            }

            try
            {
                var result = await _userService.LockUserAsync(userId);
                if (result.Succeeded)
                {
                    StatusMessage = "User locked successfully.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to lock user. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error locking user {UserId}", userId);
                StatusMessage = "Error: An unexpected error occurred while locking the user.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnlockUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Error: User ID is required.";
                return RedirectToPage();
            }

            try
            {
                var result = await _userService.UnlockUserAsync(userId);
                if (result.Succeeded)
                {
                    StatusMessage = "User unlocked successfully.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to unlock user. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unlocking user {UserId}", userId);
                StatusMessage = "Error: An unexpected error occurred while unlocking the user.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Error: User ID is required.";
                return RedirectToPage();
            }

            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                if (result.Succeeded)
                {
                    StatusMessage = "User deleted successfully.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to delete user. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                StatusMessage = "Error: An unexpected error occurred while deleting the user.";
            }

            return RedirectToPage();
        }
    }
}
