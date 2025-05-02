using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
        }
        public async Task<List<Category>> GetSubcategoriesAsync(int categoryId)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<List<int>> GetCategoryPathAsync(int categoryId)
        {
            var allCategories = await _context.Categories.ToDictionaryAsync(c => c.Id, c => c);
            var path = new List<int>();
            var currentId = categoryId;
            while (allCategories.ContainsKey(currentId))
            {
                path.Add(currentId);
                var current = allCategories[currentId];
                if (current.ParentCategoryId == null)
                    break;
                currentId = current.ParentCategoryId.Value;
            }
            path.Reverse(); // To have root first and leaf last
            return path;
        }
    }
}