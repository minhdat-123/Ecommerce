using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Blazor;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken"; // Must match the key used in ApiAuthenticationStateProvider

    public AuthHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Check if the request is targeting our API (optional, but good practice)
        // You might need to inject ApiSettings here if you want to check against _apiSettings.ApiUrl
        // For simplicity, we assume all requests handled by the named HttpClient need the token.

        var token = await _localStorage.GetItemAsync<string>(TokenKey, cancellationToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
} 