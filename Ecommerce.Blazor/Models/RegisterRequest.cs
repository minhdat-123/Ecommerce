using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Blazor.Models;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Add other fields if needed by your API (e.g., Name)
    // [Required]
    // public string FullName { get; set; } = string.Empty;
} 