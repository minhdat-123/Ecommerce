// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using IdentityService.Domain.Entities;
using IdentityService.API.Areas.Identity.Services;

namespace IdentityService.API.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IdentityUIService _identityUIService;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IdentityUIService identityUIService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _identityUIService = identityUIService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Set default return URL to Blazor application
            returnUrl ??= "https://localhost:7235";
            
            // Prevent redirection to problematic URLs, redirect to Blazor app instead
            if (returnUrl == "/" || returnUrl.Contains(".well-known"))
            {
                _logger.LogWarning($"GET: Fixed potentially problematic returnUrl: {returnUrl}");
                returnUrl = "https://localhost:7200";
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Set default return URL to Blazor application
            returnUrl ??= "https://localhost:7235";
            
            // Prevent redirection to OpenID Connect discovery document or other problematic URLs
            if (returnUrl == "/" || returnUrl.Contains(".well-known"))
            {
                _logger.LogWarning($"POST: Fixed potentially problematic returnUrl: {returnUrl}");
                returnUrl = "https://localhost:7200";
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // Use IdentityUIService to perform login via our application service layer
                var result = await _identityUIService.SignInUserAsync(Input.Email, Input.Password, Input.RememberMe);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in via application service layer.");
                    
                    // Check if we need to use external redirect
                    if (returnUrl.StartsWith("http") || returnUrl == "/" || returnUrl.Contains(".well-known"))
                    {
                        _logger.LogInformation($"Using Redirect for external URL: {returnUrl}");
                        return Redirect("https://localhost:7235");
                    }
                    
                    _logger.LogInformation($"Using LocalRedirect for: {returnUrl}");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    // Fall back to standard ASP.NET Identity sign-in if our service doesn't handle all cases
                    var standardResult = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (standardResult.Succeeded)
                    {
                        _logger.LogInformation("User logged in via standard Identity.");
                        
                        // Check if we need to use external redirect
                        if (returnUrl.StartsWith("http") || returnUrl == "/" || returnUrl.Contains(".well-known"))
                        {
                            _logger.LogInformation($"Standard flow: Using Redirect for external URL");
                            return Redirect("https://localhost:7235");
                        }
                        
                        _logger.LogInformation($"Standard flow: Using LocalRedirect for: {returnUrl}");
                        return LocalRedirect(returnUrl);
                    }
                    if (standardResult.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (standardResult.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
