using Ecommerce.Blazor.Models;

namespace Ecommerce.Blazor.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<SearchProductsResponse> SearchProductsAsync(string query = "", decimal? minPrice = null, decimal? maxPrice = null, 
            int? categoryId = null, int? parentCategoryId = null, int? brandId = null, 
            string sortBy = "", int page = 1, int pageSize = 10);
        Task<List<string>> GetSuggestionsAsync(string query);
        Task AddProductAsync(Product product);
        Task IndexProductsAsync();
    }
}