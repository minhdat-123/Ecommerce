using ProductService.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SearchConfig> SearchConfigs { get; set; }
        public DbSet<BrandCategory> BrandCategory { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<BrandCategory>()
                .HasKey(bc => new { bc.BrandId, bc.CategoryId });

            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Brand)
                .WithMany(b => b.BrandCategories)
                .HasForeignKey(bc => bc.BrandId);

            modelBuilder.Entity<BrandCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BrandCategories)
                .HasForeignKey(bc => bc.CategoryId);

            modelBuilder.Entity<SearchConfig>()
                .Property(sc => sc.Type)
                .HasConversion<int>();
        }

    }

}

