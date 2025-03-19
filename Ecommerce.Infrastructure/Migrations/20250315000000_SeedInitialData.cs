using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Execute SQL files in the correct order: categories, brands, products
            ExecuteSqlFile(migrationBuilder, "seading_categories.sql");
            ExecuteSqlFile(migrationBuilder, "seading_brand.sql");
            ExecuteSqlFile(migrationBuilder, "seading_product.sql");
        }

        private void ExecuteSqlFile(MigrationBuilder migrationBuilder, string fileNameMigration)
        {
            var assembly = typeof(SeedInitialData).Assembly;
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
            migrationBuilder.Sql("DELETE FROM Products");
            migrationBuilder.Sql("DELETE FROM BrandCategory");
            migrationBuilder.Sql("DELETE FROM Brands");
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}