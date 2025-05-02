using Ecommerce.Blazor.Models;

namespace Ecommerce.Blazor.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest model);
    Task<AuthResponse> RegisterAsync(RegisterRequest model);
} 