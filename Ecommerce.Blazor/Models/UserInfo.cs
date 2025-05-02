namespace Ecommerce.Blazor.Models;

public class UserInfo
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    // Add other relevant user fields returned by API (e.g., Name)
    // public string? FullName { get; set; }
    public List<string>? Roles { get; set; }
} 