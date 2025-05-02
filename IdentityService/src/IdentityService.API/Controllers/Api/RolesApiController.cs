using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityService.API.Controllers.Api
{
    /// <summary>
    /// API endpoints for role management that can be consumed by other services
    /// </summary>
    [ApiController]
    [Route("api/identity/roles")]
    [Authorize(Roles = "Admin")]
    public class RolesApiController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesApiController(
            IRoleService roleService,
            RoleManager<IdentityRole> roleManager)
        {
            _roleService = roleService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Gets all available roles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Checks if a role exists
        /// </summary>
        [HttpGet("exists/{roleName}")]
        public async Task<IActionResult> RoleExists(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required");
            }

            var exists = await _roleService.RoleExistsAsync(roleName);
            return Ok(new { exists });
        }
    }
}
