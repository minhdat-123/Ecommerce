using Ecommerce.Application.DTOs; // Assuming DTOs will be here
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
} 