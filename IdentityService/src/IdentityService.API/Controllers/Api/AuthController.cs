using IdentityService.Application.Interfaces;
using IdentityService.Application.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityService.API.Controllers.Api
{
    /// <summary>
    /// API endpoints for authentication that can be consumed by other services
    /// </summary>
    [ApiController]
    [Route("api/identity/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Validates user credentials
        /// </summary>
        [HttpPost("validate")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCredentials([FromBody] LoginUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(model);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { isValid = true });
        }

        /// <summary>
        /// Gets user details for authorized user
        /// </summary>
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
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
        /// Checks if a user exists by email
        /// </summary>
        [HttpGet("user-exists")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUserExists([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required");
            }

            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new { exists = user != null });
        }
    }
}
