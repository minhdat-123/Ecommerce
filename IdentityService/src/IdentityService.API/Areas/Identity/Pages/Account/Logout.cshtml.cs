// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using IdentityService.Domain.Entities;

namespace IdentityService.API.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // Add OnGet to automatically log out when the page is loaded
        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            // Default to Blazor app URL if no return URL is provided
            var blazorAppUrl = "https://localhost:7235";
            returnUrl ??= blazorAppUrl;

            // Sign the user out
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out via GET request.");

            // Always redirect back to Blazor app
            if (!returnUrl.StartsWith(blazorAppUrl) && !returnUrl.StartsWith("/"))
            {
                returnUrl = blazorAppUrl;
            }

            // Redirect to the specified URL (Blazor app)
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Default to Blazor app URL if no return URL is provided
            var blazorAppUrl = "https://localhost:7235";
            returnUrl ??= blazorAppUrl;

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            
            // Always redirect back to Blazor app
            if (!returnUrl.StartsWith(blazorAppUrl) && !returnUrl.StartsWith("/"))
            {
                returnUrl = blazorAppUrl;
            }

            // Redirect to the specified URL (Blazor app)
            return Redirect(returnUrl);
        }
    }
}
