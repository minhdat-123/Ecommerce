using Ecommerce.Blazor.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.Blazor.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiSettings _apiSettings;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    private const string ApiClientName = "AuthenticatedApiClient";

    public AuthService(IHttpClientFactory httpClientFactory, ApiSettings apiSettings)
    {
        _httpClientFactory = httpClientFactory;
        _apiSettings = apiSettings;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(ApiClientName);

    public async Task<AuthResponse> LoginAsync(LoginRequest model)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync($"{_apiSettings.ApiUrl}/auth/login", model);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(_jsonOptions);
        return authResponse ?? new AuthResponse { IsSuccess = false, Message = "Login failed. Unexpected response." };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest model)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync($"{_apiSettings.ApiUrl}/auth/register", model);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(_jsonOptions);
        return authResponse ?? new AuthResponse { IsSuccess = false, Message = "Registration failed. Unexpected response." };
    }
} 