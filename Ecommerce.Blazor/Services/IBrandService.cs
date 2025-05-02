using Ecommerce.Blazor.Models;

namespace Ecommerce.Blazor.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrandsByCategoryAsync(int categoryId);
    }
}