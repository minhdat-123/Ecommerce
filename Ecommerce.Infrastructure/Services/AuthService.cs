using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager; // Added for role management
    private readonly IJwtGenerator _jwtGenerator;

    // Define Role Names Constants
    private const string AdminRole = "Admin";
    private const string CustomerRole = "Customer";

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager, 
        IJwtGenerator jwtGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse { IsSuccess = false, Message = "Invalid credentials." };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return new AuthResponse { IsSuccess = false, Message = "Invalid credentials." };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtGenerator.GenerateToken(user, roles);

        return new AuthResponse
        {
            IsSuccess = true,
            Token = token,
            UserInfo = new UserDto { Email = user.Email, Roles = roles.ToList() }
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse { IsSuccess = false, Message = "Email already exists." };
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email // Use email as username by default
            // Set other properties if needed (FirstName, etc.)
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse { IsSuccess = false, Message = $"Registration failed: {errors}" };
        }

        // Ensure Customer role exists
        if (!await _roleManager.RoleExistsAsync(CustomerRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(CustomerRole));
        }
        // Add new user to Customer role by default
        await _userManager.AddToRoleAsync(user, CustomerRole);

        // No token returned on register by default, user should login
        return new AuthResponse { IsSuccess = true, Message = "Registration successful." };
    }

    // Helper method to seed roles (call this during app startup if needed)
    public async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync(AdminRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(AdminRole));
        }
         if (!await _roleManager.RoleExistsAsync(CustomerRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(CustomerRole));
        }
    }
} 