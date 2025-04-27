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

// --- API Gateway base URL ---
string gatewayUrl = "https://localhost:7200";

// --- Identity Service base URL (for direct authentication flows) ---
string identityServiceUrl = "https://localhost:7273";

// Output all JWT claims for debugging
Console.WriteLine("Initializing application with debugging enabled...");

// --- Configure HttpClient to point to the API Gateway for regular API calls --- 
builder.Services.AddHttpClient("API", client => 
    client.BaseAddress = new Uri($"{gatewayUrl}/api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>(); // OIDC handler

// --- Configure a separate HttpClient specifically for Identity API calls through gateway ---
builder.Services.AddHttpClient("IdentityAPI", client => 
    client.BaseAddress = new Uri($"{gatewayUrl}/api/identity/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient for components that use it directly
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// --- Supply ApiSettings ---
builder.Services.AddSingleton(new ApiSettings { 
    ApiUrl = $"{gatewayUrl}/api/", 
    IdentityApiUrl = $"{gatewayUrl}/api/identity/"
});

// --- Add OIDC Authentication (direct to Identity Service) --- 
builder.Services.AddOidcAuthentication(options =>
{
    // Configure your OIDC provider (IdentityService.API)
    // This is a direct connection to the Identity Service, not through the gateway
    options.ProviderOptions.Authority = identityServiceUrl; 
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
    // Add an AdminPolicy that requires the Administrator role
    options.AddPolicy("AdminPolicy", policy => 
        policy.RequireRole("Administrator"));
});

// Register Blazored Local Storage (can still be useful)
builder.Services.AddBlazoredLocalStorage();

// Register Authorization Core services (needed for AuthorizeView etc.)
builder.Services.AddAuthorizationCore(); 

// Register application services (these should now use the IHttpClientFactory injected client)
builder.Services.AddScoped<IProductService, Ecommerce.Blazor.Services.ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
// Add Identity service for user profile operations via API gateway
builder.Services.AddScoped<IIdentityService, IdentityService>();

await builder.Build().RunAsync();