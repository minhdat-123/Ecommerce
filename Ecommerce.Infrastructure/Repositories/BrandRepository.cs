using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetBrandsByCategoryIdAsync(int categoryId)
        {
            return await _context.Brands
                .Where(b => b.BrandCategories.Any(bc => bc.CategoryId == categoryId))
                .ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }
    }
}