using IdentityModel;
using IdentityService.Domain.Entities;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace IdentityService.API.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly ILogger<CustomProfileService> _logger;

        public CustomProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            ILogger<CustomProfileService> logger)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _logger = logger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null)
            {
                _logger.LogWarning("No subject ID claim present");
                return;
            }

            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                _logger.LogWarning("User with ID {0} not found", sub);
                return;
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            // Add role claims specifically
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
                // Also add as regular ASP.NET role claim for backward compatibility
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add email claim - this may be important for some applications
            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            }

            // Add to issued claims for debugging
            _logger.LogInformation("Issuing claims for user {0}: {1}", 
                user.UserName, 
                string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}")));

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (sub == null)
            {
                _logger.LogWarning("No subject ID claim present");
                context.IsActive = false;
                return;
            }

            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
} 