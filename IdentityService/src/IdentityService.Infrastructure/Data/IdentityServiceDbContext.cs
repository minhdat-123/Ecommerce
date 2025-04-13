using IdentityService.Domain.Entities; // Restore this using
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

// Inherit from IdentityDbContext<ApplicationUser> to get Identity features
public class IdentityServiceDbContext : IdentityDbContext<ApplicationUser> 
{
    // Explicitly define DbSet for ApplicationUser to ensure it's recognized
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // IMPORTANT: Call base.OnModelCreating FIRST to configure the Identity model
        base.OnModelCreating(builder);
        
        // Explicitly configure ApplicationUser entity to ensure it's properly included in the model
        builder.Entity<ApplicationUser>(entity => {
            entity.ToTable("AspNetUsers");
            // Primary key
            entity.HasKey(e => e.Id);
            // Ensure properties are properly mapped
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
        });

        // Explicitly configure relationships for user roles
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        // Explicitly configure relationships for user claims
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Explicitly configure relationships for user logins
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        // Explicitly configure relationships for user tokens
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });
    }
} 