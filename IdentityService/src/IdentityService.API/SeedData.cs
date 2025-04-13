using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using IdentityService.Domain.Entities; // Restore this using

namespace IdentityService.API;

public class SeedData
{
    // Synchronous version for the first seeding call
    public static void EnsureSeedData(WebApplication app)
    {
        EnsureSeedDataAsync(app).GetAwaiter().GetResult();
    }

    public static async Task EnsureSeedDataAsync(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<Infrastructure.Data.IdentityServiceDbContext>();
            // Apply migrations automatically (optional, could also be done manually)
            // context.Database.Migrate(); 
            // Note: Depending on deployment strategy, applying migrations here might not be ideal.
            // It's often better to apply them as a separate deployment step.
            // Ensure the DB is created if it doesn't exist, especially for development.
            await context.Database.EnsureCreatedAsync();

            // Use ApplicationUser
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Log.Information("Seeding database...");

            // Seed Roles
            await EnsureRoleAsync(roleMgr, "Admin");
            await EnsureRoleAsync(roleMgr, "Customer");

            // Seed Admin User
            await EnsureUserAsync(userMgr, "admin@ecom.com", "Dicentral@9", "Admin");

            Log.Information("Done seeding database.");
        }
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleMgr, string roleName)
    {
        if (!await roleMgr.RoleExistsAsync(roleName))
        {
            Log.Debug($"Creating role {roleName}");
            var role = new IdentityRole(roleName);
            var result = await roleMgr.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
             Log.Debug($"Role {roleName} created");
        }
         else
        {
            Log.Debug($"Role {roleName} already exists");
        }
    }

    private static async Task EnsureUserAsync(UserManager<ApplicationUser> userMgr, string email, string password, string roleName)
    {
        var user = await userMgr.FindByEmailAsync(email);
        if (user == null)
        {
            Log.Debug($"Creating user {email}");
            // Use ApplicationUser
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await userMgr.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userMgr.AddToRoleAsync(user, roleName);
             if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug($"User {email} created and assigned to role {roleName}");
        }
        else
        {
             Log.Debug($"User {email} already exists");
             // Optionally ensure role assignment even if user exists
             if (!await userMgr.IsInRoleAsync(user, roleName))
             {
                 Log.Debug($"Assigning existing user {email} to role {roleName}");
                 var roleResult = await userMgr.AddToRoleAsync(user, roleName);
                 if (!roleResult.Succeeded)
                 {
                     throw new Exception(roleResult.Errors.First().Description);
                 }
             }
        }
    }
} 