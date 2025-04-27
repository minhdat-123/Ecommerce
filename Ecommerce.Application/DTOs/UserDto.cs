using System.Collections.Generic;

namespace ProductService.Application.DTOs;

// Similar to UserInfo in Blazor, but maybe without sensitive info like ID depending on needs
public class UserDto
{
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
    // Add other relevant fields (FirstName, etc.)
} 
