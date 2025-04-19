using System.Security.Claims;

namespace Ecommerce.Blazor;

public static class AdminRoleProvider
{
    private const string AdminEmail = "admin@ecom.com";

    public static bool IsUserAdmin(ClaimsPrincipal user)
    {
        // If the user is not authenticated, they cannot be an admin
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return false;
        }

        // Check for explicit role claims (standard approach)
        if (user.IsInRole("Administrator") || user.IsInRole("Admin"))
        {
            return true;
        }

        // Special case: hardcoded admin email check
        // This is a fallback when role claims are not working correctly
        var emailClaim = user.FindFirst(ClaimTypes.Email) ?? 
                         user.FindFirst("email") ??
                         user.Claims.FirstOrDefault(c => c.Type.Contains("email"));
        
        return emailClaim != null && emailClaim.Value.Equals(AdminEmail, StringComparison.OrdinalIgnoreCase);
    }
}
