using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<MigrationController> _logger;

        public MigrationController(AppDbContext dbContext, ILogger<MigrationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Automatically creates and seeds the database
        /// </summary>
        /// <returns>Status of the migration operation</returns>
        [HttpGet]
        public async Task<IActionResult> MigrateDatabase()
        {
            try
            {
                _logger.LogInformation("Starting database migration and seeding process");
                
                // Check if database exists, if not it will be created when migrations are applied
                bool dbExists = await _dbContext.Database.CanConnectAsync();
                _logger.LogInformation(dbExists ? "Database already exists" : "Database does not exist, will be created");

                // Apply all pending migrations including the SeedInitialData migration
                _logger.LogInformation("Applying pending migrations");
                int pendingMigrations = (await _dbContext.Database.GetPendingMigrationsAsync()).Count();
                
                if (pendingMigrations > 0)
                {
                    await _dbContext.Database.MigrateAsync();
                    _logger.LogInformation($"Successfully applied {pendingMigrations} pending migrations");
                    return Ok(new { Success = true, Message = $"Database created and seeded successfully. Applied {pendingMigrations} migrations." });
                }
                else
                {
                    _logger.LogInformation("No pending migrations to apply");
                    return Ok(new { Success = true, Message = "Database is up to date. No migrations were applied." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database migration");
                return StatusCode(500, new { Success = false, Message = $"Error during database migration: {ex.Message}" });
            }
        }
    }
}