using Blazored.LocalStorage;
using Ecommerce.Blazor;
using Ecommerce.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the DelegatingHandler
builder.Services.AddTransient<AuthHeaderHandler>();

// Configure a named HttpClient for authenticated API calls
builder.Services.AddHttpClient("AuthenticatedApiClient", client =>
{
    // Base address can be set here if needed, or rely on ApiSettings injection in services
    // client.BaseAddress = new Uri("https://localhost:7233/api/"); 
})
.AddHttpMessageHandler<AuthHeaderHandler>();

// Configure the default HttpClient (used by ApiAuthenticationStateProvider and potentially others)
// It won't automatically have the auth header unless ApiAuthenticationStateProvider sets it.
builder.Services.AddScoped(sp => 
{
    // Optionally configure the default client if needed, otherwise just create it.
    var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    return client;
    // --- OR --- 
    // Return the named client IF all services will use it and ApiAuthenticationStateProvider
    // gets the IHttpClientFactory to retrieve the named client.
    // var factory = sp.GetRequiredService<IHttpClientFactory>();
    // return factory.CreateClient("AuthenticatedApiClient"); 
});

// Register API URL as a service
builder.Services.AddSingleton(new ApiSettings { ApiUrl = "https://localhost:7233/api" });

// Register Blazored Local Storage
builder.Services.AddBlazoredLocalStorage();

// Register Authorization Core services
builder.Services.AddAuthorizationCore();

// Register custom AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

// Register services - IMPORTANT: Inject IHttpClientFactory and use CreateClient("AuthenticatedApiClient")
// OR change the default registration above to return the named client.
// For simplicity, let's modify services to use IHttpClientFactory.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();