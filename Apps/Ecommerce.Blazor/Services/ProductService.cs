using Ecommerce.Blazor.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Blazor.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string ApiClientName = "AuthenticatedApiClient";

        public ProductService(IHttpClientFactory httpClientFactory, ApiSettings apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient(ApiClientName);

        private string BuildApiUrl(string path)
        {
            // Ensures no double slashes in the final URL
            return _apiSettings.ApiUrl.TrimEnd('/') + "/" + path.TrimStart('/');
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync(BuildApiUrl("products"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Product>>(_jsonOptions) ?? new List<Product>();
        }

        public async Task<SearchProductsResponse> SearchProductsAsync(string query = "", decimal? minPrice = null, decimal? maxPrice = null,
            int? categoryId = null, int? parentCategoryId = null, int? brandId = null,
            string sortBy = "", int page = 1, int pageSize = 10)
        {
            var client = CreateClient();
            var requestUri = BuildApiUrl("products/search?");
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(query))
                queryParams.Add($"query={Uri.EscapeDataString(query)}");
            if (minPrice.HasValue)
                queryParams.Add($"minPrice={minPrice.Value}");
            if (maxPrice.HasValue)
                queryParams.Add($"maxPrice={maxPrice.Value}");
            if (categoryId.HasValue)
                queryParams.Add($"categoryId={categoryId.Value}");
            if (parentCategoryId.HasValue)
                queryParams.Add($"parentCategoryId={parentCategoryId.Value}");
            if (brandId.HasValue)
                queryParams.Add($"brandId={brandId.Value}");
            if (!string.IsNullOrEmpty(sortBy))
                queryParams.Add($"sortBy={Uri.EscapeDataString(sortBy)}");

            queryParams.Add($"page={page}");
            queryParams.Add($"pageSize={pageSize}");

            requestUri += string.Join("&", queryParams);

            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SearchProductsResponse>(_jsonOptions) ?? new SearchProductsResponse();
        }

        public async Task<List<string>> GetSuggestionsAsync(string query)
        {
            var client = CreateClient();
            if (string.IsNullOrEmpty(query))
                return new List<string>();

            var requestUri = BuildApiUrl($"products/suggestions?query={Uri.EscapeDataString(query)}");
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<string>>(_jsonOptions) ?? new List<string>();
        }

        public async Task AddProductAsync(Product product)
        {
            var client = CreateClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = product.Name,
                    description = product.Description,
                    price = product.Price,
                    categoryId = product.CategoryId,
                    brandId = product.BrandId
                }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(BuildApiUrl("products"), content);
            response.EnsureSuccessStatusCode();
        }

        public async Task IndexProductsAsync()
        {
            var client = CreateClient();
            var response = await client.PostAsync(BuildApiUrl("products/index"), null);
            response.EnsureSuccessStatusCode();
        }
    }
}