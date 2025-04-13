using Blazored.LocalStorage;
using Ecommerce.Blazor;
using Ecommerce.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// --- Configure HttpClient to point to the API Gateway --- 
// Base address for API calls via the gateway
string gatewayApiUrl = "https://localhost:7200/api"; // Use the Gateway HTTPS URL

builder.Services.AddHttpClient("API", client => 
    client.BaseAddress = new Uri(gatewayApiUrl))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>(); // OIDC handler

// Supply HttpClient for components that use it directly
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// --- Restore ApiSettings pointing to Gateway ---
builder.Services.AddSingleton(new ApiSettings { ApiUrl = gatewayApiUrl });

// --- Remove old custom auth setup ---
// builder.Services.AddTransient<AuthHeaderHandler>(); // Removed
// builder.Services.AddSingleton(new ApiSettings { ApiUrl = "https://localhost:7200/api" }); // Old line removed previously
// builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>(); // Removed

// --- Add OIDC Authentication --- 
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your OIDC provider (IdentityService.API)
    // Ensure this matches the HTTPS URL from IdentityService.API launchSettings.json
    options.ProviderOptions.Authority = "https://localhost:7273"; 
    options.ProviderOptions.ClientId = "blazor_wasm"; // Client ID defined in IdentityService Config.cs
    options.ProviderOptions.ResponseType = "code"; // Use Authorization Code Flow

    // Define scopes needed by the Blazor app
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    options.ProviderOptions.DefaultScopes.Add("ecommerce.api"); // Scope to access the API
    // options.ProviderOptions.DefaultScopes.Add("offline_access"); // If refresh tokens needed

    // Map claims correctly for user info display
    options.UserOptions.NameClaim = "name";
    options.UserOptions.RoleClaim = "role"; // Default is "role", can also use ClaimTypes.Role
});

// Configure authorization policies
builder.Services.AddAuthorizationCore(options => 
{
    // Add an AdminPolicy that requires the Admin role
    options.AddPolicy("AdminPolicy", policy => 
        policy.RequireRole("Admin"));
});

// Register Blazored Local Storage (can still be useful)
builder.Services.AddBlazoredLocalStorage();

// Register Authorization Core services (needed for AuthorizeView etc.)
builder.Services.AddAuthorizationCore(); 

// Register application services (these should now use the IHttpClientFactory injected client)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // This might need adjustment or removal

await builder.Build().RunAsync();