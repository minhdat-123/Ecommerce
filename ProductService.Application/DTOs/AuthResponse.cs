namespace ProductService.Application.DTOs;

public class AuthResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public UserDto? UserInfo { get; set; }
    public string? Token { get; set; }
    // public DateTime? TokenExpiration { get; set; } // Optional
} 
