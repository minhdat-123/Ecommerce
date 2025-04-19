using IdentityService.Application.Interfaces;
using IdentityService.Application.Models;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.API.Areas.Identity.Services
{
    /// <summary>
    /// Adapter service to bridge ASP.NET Core Identity UI with our clean architecture application layer
    /// </summary>
    public class IdentityUIService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityUIService(
            IUserService userService,
            IRoleService roleService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Converts ASP.NET Identity result to application layer success/error format
        /// </summary>
        public async Task<(bool Succeeded, string[] Errors)> RegisterUserAsync(string userName, string email, string password)
        {
            var registerDto = new RegisterUserDto
            {
                UserName = userName,
                Email = email,
                Password = password,
                ConfirmPassword = password
            };

            return await _userService.RegisterUserAsync(registerDto);
        }

        /// <summary>
        /// Gets user by ID using application layer service
        /// </summary>
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            return await _userService.GetUserByIdAsync(userId);
        }

        /// <summary>
        /// Updates user using application layer service
        /// </summary>
        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(string userId, UserDto userDto)
        {
            return await _userService.UpdateUserAsync(userId, userDto);
        }

        /// <summary>
        /// Converts identity sign-in result to application service format
        /// </summary>
        public async Task<(bool Succeeded, string Token)> SignInUserAsync(string userName, string password, bool rememberMe)
        {
            var loginDto = new LoginUserDto
            {
                UserNameOrEmail = userName,
                Password = password,
                RememberMe = rememberMe
            };

            return await _userService.LoginAsync(loginDto);
        }

        /// <summary>
        /// Gets all roles using application layer service
        /// </summary>
        public async Task<string[]> GetAllRolesAsync()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return roles?.ToArray() ?? Array.Empty<string>();
        }
    }
}
