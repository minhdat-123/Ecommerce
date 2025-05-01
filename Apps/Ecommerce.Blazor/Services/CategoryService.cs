using Ecommerce.Blazor.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.Blazor.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string ApiClientName = "AuthenticatedApiClient";

        public CategoryService(IHttpClientFactory httpClientFactory, ApiSettings apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient(ApiClientName);

        private string BuildApiUrl(string path)
        {
            return _apiSettings.ApiUrl.TrimEnd('/') + "/" + path.TrimStart('/');
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BuildApiUrl("categories"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>() ?? new List<Category>();
        }

        public async Task<List<Category>> GetTopLevelCategoriesAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BuildApiUrl("categories/top-level"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>() ?? new List<Category>();
        }

        public async Task<List<Category>> GetSubcategoriesAsync(int parentCategoryId)
        {
            var client = CreateClient();
            var response = await client.GetAsync(BuildApiUrl($"categories/subcategories/{parentCategoryId}"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Category>>() ?? new List<Category>();
        }
    }
}