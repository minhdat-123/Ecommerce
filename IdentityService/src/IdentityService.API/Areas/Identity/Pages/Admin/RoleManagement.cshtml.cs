using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Interfaces;
using IdentityService.API.Areas.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.API.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class RoleManagementModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly IdentityUIService _identityUIService;
        private readonly ILogger<RoleManagementModel> _logger;

        public RoleManagementModel(
            IRoleService roleService,
            IdentityUIService identityUIService,
            ILogger<RoleManagementModel> logger)
        {
            _roleService = roleService;
            _identityUIService = identityUIService;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<string> Roles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                Roles = roles.ToList();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading role management page");
                StatusMessage = "Error: Failed to load roles. Please try again later.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                if (Input.RoleName.Contains(" "))
                {
                    ModelState.AddModelError(string.Empty, "Role names cannot contain spaces.");
                    await OnGetAsync();
                    return Page();
                }

                var result = await _roleService.CreateRoleAsync(Input.RoleName);
                if (result.Succeeded)
                {
                    StatusMessage = $"Role '{Input.RoleName}' created successfully.";
                    Input.RoleName = string.Empty;
                }
                else
                {
                    StatusMessage = $"Error: Failed to create role. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role {RoleName}", Input.RoleName);
                StatusMessage = "Error: An unexpected error occurred while creating the role.";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                StatusMessage = "Error: Role name is required.";
                return RedirectToPage();
            }

            try
            {
                if (roleName == "Administrator")
                {
                    StatusMessage = "Error: The Administrator role cannot be deleted.";
                    return RedirectToPage();
                }

                var result = await _roleService.DeleteRoleAsync(roleName);
                if (result.Succeeded)
                {
                    StatusMessage = $"Role '{roleName}' deleted successfully.";
                }
                else
                {
                    StatusMessage = $"Error: Failed to delete role. {string.Join(", ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {RoleName}", roleName);
                StatusMessage = "Error: An unexpected error occurred while deleting the role.";
            }

            return RedirectToPage();
        }
    }
}
