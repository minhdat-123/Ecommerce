namespace Ecommerce.Blazor.Models;

public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public UserInfo? UserInfo { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiration { get; set; } // Optional: if API provides it
} 