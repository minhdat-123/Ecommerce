using Ecommerce.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.Blazor.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public CategoryService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.ApiUrl}/categories");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>(_jsonOptions) ?? new List<Category>();
        }

        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.ApiUrl}/categories/top-level");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>(_jsonOptions) ?? new List<Category>();
        }

        public async Task<List<Category>> GetSubcategoriesAsync(int parentCategoryId)
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.ApiUrl}/categories/subcategories/{parentCategoryId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>(_jsonOptions) ?? new List<Category>();
        }
    }
}