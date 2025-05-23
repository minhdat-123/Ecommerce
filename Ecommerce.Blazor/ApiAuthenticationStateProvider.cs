using Blazored.LocalStorage;
using Ecommerce.Blazor.Models; // Assuming UserInfo is here
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Ecommerce.Blazor;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient; // Needed to update Authorization header
    private readonly JwtSecurityTokenHandler _jwtHandler = new();

    private const string TokenKey = "authToken";

    public ApiAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>(TokenKey);

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Parse token and check expiry (optional but recommended)
        try
        {
            var token = _jwtHandler.ReadJwtToken(savedToken);
            if (token.ValidTo < DateTime.UtcNow)
            {
                await MarkUserAsLoggedOut(); // Token expired
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt");
            var user = new ClaimsPrincipal(identity);
            
            // Set default authorization header for subsequent requests
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            return new AuthenticationState(user);
        }
        catch (Exception ex)
        {
             Console.WriteLine($"Error parsing token: {ex.Message}");
             await MarkUserAsLoggedOut(); // Invalid token
             return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return;

        await _localStorage.SetItemAsync(TokenKey, token);
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        _httpClient.DefaultRequestHeaders.Authorization = null;

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        try
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                // Extract standard claims 
                keyValuePairs.TryGetValue(JwtRegisteredClaimNames.NameId, out object? id);
                if (id != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString() ?? string.Empty));

                keyValuePairs.TryGetValue(JwtRegisteredClaimNames.Email, out object? email);
                if (email != null) claims.Add(new Claim(ClaimTypes.Email, email.ToString() ?? string.Empty));
                
                keyValuePairs.TryGetValue(JwtRegisteredClaimNames.GivenName, out object? name); 
                if (name != null) claims.Add(new Claim(ClaimTypes.Name, name.ToString() ?? string.Empty));

                // ---- Flexible Role Claim Extraction ----
                // Try standard ClaimTypes.Role first
                if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object? rolesClaim))
                {
                    AddRolesToClaims(claims, rolesClaim);
                }
                // If not found, try the simple name "role"
                else if (keyValuePairs.TryGetValue("role", out object? simpleRolesClaim))
                {
                    AddRolesToClaims(claims, simpleRolesClaim);
                }
                 // If not found, try the simple name "roles"
                else if (keyValuePairs.TryGetValue("roles", out object? simpleRolesClaimPlural))
                {
                    AddRolesToClaims(claims, simpleRolesClaimPlural);
                }
                // ---- End Flexible Role Claim Extraction ----
            }
        }
        catch (Exception ex)
        {
             Console.WriteLine($"Error parsing claims from JWT: {ex.Message}");
        }
        return claims;
    }

    // Helper method to handle adding single string or array of roles
    private void AddRolesToClaims(List<Claim> claims, object? rolesClaim)
    {
        if (rolesClaim is JsonElement rolesElement && rolesElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var role in rolesElement.EnumerateArray())
            {
                if (role.ValueKind == JsonValueKind.String)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.GetString() ?? string.Empty));
                }
            }
        }
        else if (rolesClaim != null)
        {
            // Assume single role string
            claims.Add(new Claim(ClaimTypes.Role, rolesClaim.ToString() ?? string.Empty));
        }
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
} 