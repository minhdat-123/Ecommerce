using ProductService.Domain.Entities;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetTopLevelCategoriesAsync();
        Task<List<Category>> GetSubcategoriesAsync(int categoryId);
        Task<List<int>> GetCategoryPathAsync(int categoryId);
    }
}
