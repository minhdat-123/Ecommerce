using Ecommerce.Blazor.Models;

namespace Ecommerce.Blazor.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Category>> GetTopLevelCategoriesAsync();
        Task<List<Category>> GetSubcategoriesAsync(int parentCategoryId);
    }
}