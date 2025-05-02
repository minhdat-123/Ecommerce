using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Domain.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Add custom properties here if needed, e.g.:
    // public string? FirstName { get; set; }
    // public string? LastName { get; set; }
} 