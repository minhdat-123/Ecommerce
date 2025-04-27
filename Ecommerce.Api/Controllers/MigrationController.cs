using ProductService.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using ProductService.Domain.Entities; // Ensure this is present
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<MigrationController> _logger;

        public MigrationController(
            AppDbContext dbContext,
            UserManager<ApplicationUser> userManager, // Inject UserManager
            RoleManager<IdentityRole> roleManager,   // Inject RoleManager
            ILogger<MigrationController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Applies pending migrations and ensures the admin user is correctly configured.
        /// </summary>
        /// <returns>Status of the migration and seeding operation</returns>
        [HttpGet]
        public async Task<IActionResult> MigrateDatabase()
        {
            try
            {
                _logger.LogInformation("Starting database migration and admin user check process");

                // Check if database exists
                bool dbExists = await _dbContext.Database.CanConnectAsync();
                _logger.LogInformation(dbExists ? "Database already exists" : "Database does not exist, will be created");

                // Apply all pending migrations
                _logger.LogInformation("Applying pending migrations...");
                int pendingMigrations = (await _dbContext.Database.GetPendingMigrationsAsync()).Count();
                string migrationMessage;

                if (pendingMigrations > 0)
                {
                    await _dbContext.Database.MigrateAsync();
                    migrationMessage = $"Successfully applied {pendingMigrations} pending migrations.";
                    _logger.LogInformation(migrationMessage);
                }
                else
                {
                    migrationMessage = "No pending migrations to apply.";
                    _logger.LogInformation(migrationMessage);
                }

                // Ensure Admin User exists and password is correct
                _logger.LogInformation("Ensuring admin user exists and password is correct...");
                string adminSetupMessage = await EnsureAdminUserAsync();
                _logger.LogInformation(adminSetupMessage);

                return Ok(new { Success = true, Message = $"{migrationMessage} {adminSetupMessage}" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database migration or admin user setup");
                return StatusCode(500, new { Success = false, Message = $"Error during operation: {ex.Message}" });
            }
        }

        private async Task<string> EnsureAdminUserAsync()
        {
            const string adminEmail = "admin@ecom.com";
            const string adminPassword = "Dicentral@9"; // The expected password
            const string adminRole = "Admin";

            // Ensure roles exist (important if DB was created fresh)
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
                 _logger.LogInformation($"Created '{adminRole}' role.");
            }
             if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
                 _logger.LogInformation("Created 'Customer' role.");
            }

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // This case should ideally not happen if the SQL migration ran correctly,
                // but we handle it defensively.
                _logger.LogWarning($"Admin user '{adminEmail}' not found after migration. Attempting to create.");
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    _logger.LogError($"Failed to create admin user '{adminEmail}': {errors}");
                    return $"Failed to create admin user: {errors}";
                }
                await _userManager.AddToRoleAsync(adminUser, adminRole);
                return $"Admin user '{adminEmail}' created and assigned to '{adminRole}' role.";
            }
            else
            {
                // User exists, check password and role
                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(adminUser, adminPassword);
                if (!isPasswordCorrect)
                {
                    _logger.LogWarning($"Admin user '{adminEmail}' password requires update.");
                    // Remove old password hash and add the new one
                    var removeResult = await _userManager.RemovePasswordAsync(adminUser);
                    if (removeResult.Succeeded)
                    {
                        var addResult = await _userManager.AddPasswordAsync(adminUser, adminPassword);
                        if (!addResult.Succeeded)
                        {
                             var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                             _logger.LogError($"Failed to update admin user '{adminEmail}' password: {errors}");
                             return $"Failed to update admin password: {errors}";
                        }
                         _logger.LogInformation($"Admin user '{adminEmail}' password updated successfully.");
                    }
                    else
                    {
                         var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                         _logger.LogError($"Failed to remove old password for admin user '{adminEmail}': {errors}");
                         return $"Failed to remove old password: {errors}";
                    }
                }

                // Ensure user is in Admin role
                if (!await _userManager.IsInRoleAsync(adminUser, adminRole))
                {                 
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                    _logger.LogInformation($"Assigned admin user '{adminEmail}' to '{adminRole}' role.");
                    return $"Admin user '{adminEmail}' exists, password verified, and assigned to '{adminRole}' role.";
                }
                 return $"Admin user '{adminEmail}' exists, password verified, and role is correct.";
            }
        }
    }
}
