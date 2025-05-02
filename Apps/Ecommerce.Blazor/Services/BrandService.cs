using Ecommerce.Blazor.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.Blazor.Services
{
    public class BrandService : IBrandService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string ApiClientName = "AuthenticatedApiClient";

        public BrandService(IHttpClientFactory httpClientFactory, ApiSettings apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient(ApiClientName);

        private string BuildApiUrl(string path)
        {
            return _apiSettings.ApiUrl.TrimEnd('/') + "/" + path.TrimStart('/');
        }

        public async Task<List<Brand>> GetBrandsByCategoryAsync(int categoryId)
        {
            var client = CreateClient();
            var response = await client.GetAsync(BuildApiUrl($"Brand/category/{categoryId}"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Brand>>(_jsonOptions) ?? new List<Brand>();
        }
    }
}