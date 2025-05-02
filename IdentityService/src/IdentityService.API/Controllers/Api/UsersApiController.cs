using IdentityService.Application.Interfaces;
using IdentityService.Application.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.API.Controllers.Api
{
    /// <summary>
    /// API endpoints for user management that can be consumed through the API gateway
    /// </summary>
    [ApiController]
    [Route("api/identity/users")]
    [Authorize]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersApiController(
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all users (admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            // Only allow admins or the user themselves to access their data
            if (!User.IsInRole("Admin") && User.FindFirst("sub")?.Value != id)
            {
                return Forbid();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets the current user's profile
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = (await _userManager.FindByEmailAsync(model.Email)).Id }, null);
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto model)
        {
            // Only allow admins or the user themselves to update their data
            if (!User.IsInRole("Admin") && User.FindFirst("sub")?.Value != id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(id, model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a user (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Gets a user's roles
        /// </summary>
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            // Only allow admins or the user themselves to see their roles
            if (!User.IsInRole("Admin") && User.FindFirst("sub")?.Value != userId)
            {
                return Forbid();
            }

            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        /// <summary>
        /// Adds a user to a role (admin only)
        /// </summary>
        [HttpPost("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(string userId, [FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required");
            }

            var result = await _userService.AssignUserToRoleAsync(userId, roleName);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Removes a user from a role (admin only)
        /// </summary>
        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            var result = await _userService.RemoveUserFromRoleAsync(userId, roleName);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
