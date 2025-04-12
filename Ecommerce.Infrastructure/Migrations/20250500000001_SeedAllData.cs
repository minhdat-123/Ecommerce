using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
// Remove using for Identity/Entities if no longer needed directly in this file
// using Microsoft.AspNetCore.Identity;
// using Ecommerce.Domain.Entities;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllData : Migration
    {
        // Remove constructor and private field for IPasswordHasher
        // private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        // public SeedAllData(IPasswordHasher<ApplicationUser> passwordHasher)
        // {
        //     _passwordHasher = passwordHasher;
        // }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Execute SQL files for base data
            ExecuteSqlFile(migrationBuilder, "seading_categories.sql");
            ExecuteSqlFile(migrationBuilder, "seading_brand.sql");
            ExecuteSqlFile(migrationBuilder, "seading_product.sql");

            // Execute SQL for creating roles (from seeding_admin_user.sql)
            ExecuteSqlFile(migrationBuilder, "seeding_admin_user.sql");

            // --- User creation is now handled in MigrationController ---
        }

        private void ExecuteSqlFile(MigrationBuilder migrationBuilder, string fileNameMigration)
        {
            var assembly = typeof(SeedAllData).Assembly;
            var manifestResourceNames = assembly.GetManifestResourceNames();
            var fileName = Array.Find(manifestResourceNames, fileName =>
                fileName.StartsWith("Ecommerce.Infrastructure", StringComparison.InvariantCultureIgnoreCase) &&
                fileName.EndsWith(fileNameMigration, StringComparison.InvariantCultureIgnoreCase)
            );
            
            if (fileName == null)
            {
                throw new FileNotFoundException($"SQL file not found as embedded resource: {fileNameMigration}");
            }
            
            using var stream = assembly.GetManifestResourceStream(fileName) ?? throw new InvalidOperationException($"Could not load resource stream for {fileName}");
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Clear the seeded data in reverse order
            migrationBuilder.Sql(@"
                -- Remove admin user (role assignment handled by Identity)
                DELETE FROM AspNetUsers WHERE Email = 'admin@ecom.com';

                -- Clear roles (optional, depends if you want to keep them)
                -- DELETE FROM AspNetRoles WHERE Name IN ('Admin', 'Customer');

                -- Clear products, brands and categories
                DELETE FROM Orders;
                DELETE FROM Products;
                DELETE FROM BrandCategory;
                DELETE FROM Brands;
                DELETE FROM Categories;
            ");
        }
    }
} 