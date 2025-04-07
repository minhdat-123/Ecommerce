using Ecommerce.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.Blazor.Services
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public BrandService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<List<Brand>> GetBrandsByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.ApiUrl}/Brand/category/{categoryId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Brand>>(_jsonOptions) ?? new List<Brand>();
        }
    }
}