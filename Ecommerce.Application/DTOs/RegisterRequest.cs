using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    // Add other fields if needed for registration (e.g., FirstName, LastName)
    // public string? FirstName { get; set; }
    // public string? LastName { get; set; }
} 