// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityService.API.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;
        private readonly IUserService _userService;

        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger,
            IUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Use our application service to get user data if possible
            var userDto = await _userService.GetUserByIdAsync(user.Id);
            Dictionary<string, string> personalData;
            
            if (userDto != null)
            {
                // Use the data from our service
                personalData = new Dictionary<string, string>
                {
                    { "Id", userDto.Id },
                    { "UserName", userDto.UserName },
                    { "Email", userDto.Email },
                    { "PhoneNumber", userDto.PhoneNumber },
                    { "EmailConfirmed", userDto.EmailConfirmed.ToString() },
                    { "PhoneNumberConfirmed", userDto.PhoneNumberConfirmed.ToString() },
                    { "TwoFactorEnabled", userDto.TwoFactorEnabled.ToString() },
                    { "Roles", string.Join(", ", userDto.Roles) }
                };
            }
            else
            {
                // Fallback to standard UserManager if service fails
                // Only export personal data (not logins, roles, etc.)
                var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
                personalData = personalDataProps.ToDictionary(p => p.Name, p => p.GetValue(user)?.ToString() ?? "null");

                // Add roles manually
                var roles = await _userManager.GetRolesAsync(user);
                personalData.Add("Roles", string.Join(", ", roles));
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}
