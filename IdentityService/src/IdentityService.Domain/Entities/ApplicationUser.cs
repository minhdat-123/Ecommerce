using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

// Represents the user in the Identity system
public class ApplicationUser : IdentityUser
{
    // Add custom properties specific to Identity context if needed in the future
    // e.g., public string? ProfilePictureUrl { get; set; }
} 