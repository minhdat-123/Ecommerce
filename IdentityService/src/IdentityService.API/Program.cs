using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer;
using Duende.IdentityServer.Models; // Required for ApiScope, Client, etc.
// using Duende.IdentityServer.Test; // Remove if not using test users
using System.Security.Claims;
using Serilog; // Add Serilog
using IdentityService.API; // Add using for SeedData

// Configure Serilog early
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting IdentityService API");

try 
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog from appsettings/code
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // 1. Add DbContext for Identity
    var connectionString = builder.Configuration.GetConnectionString("IdentityConnection") ?? throw new InvalidOperationException("Connection string 'IdentityConnection' not found.");
    builder.Services.AddDbContext<IdentityServiceDbContext>(options =>
        options.UseSqlServer(connectionString));

    // 2. Add ASP.NET Core Identity (using ApplicationUser)
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Example: Configure password settings if needed
            options.SignIn.RequireConfirmedAccount = false; // Set to true if email confirmation is desired
        })
        .AddEntityFrameworkStores<IdentityServiceDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

    // 3. Add Duende IdentityServer
    builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // See https://docs.duendesoftware.com/identityserver/v7/fundamentals/resources/
            // options.EmitStaticAudienceClaim = true;
        })
        // In-memory stores for quick start - Replace with EF Core stores for production
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiScopes(Config.GetApiScopes())
        .AddInMemoryClients(Config.GetClients(builder.Configuration)) // Pass config
        // Use ApplicationUser
        .AddAspNetIdentity<ApplicationUser>(); // Integrate with ASP.NET Core Identity

    // Add services to the container.
    builder.Services.AddControllersWithViews(); // If you want a UI for login etc.
    builder.Services.AddRazorPages(); // Add Razor Pages services
    
    var app = builder.Build();

    // Seed data (only in Development environment)
    if (app.Environment.IsDevelopment())
    {
        // Uncomment seeding to create admin user
        SeedData.EnsureSeedData(app);
        Log.Information("Database seeding is enabled.");
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        // Configure for production (e.g., UseExceptionHandler, UseHsts)
    }
    
    // app.UseHttpsRedirection(); 
    app.UseStaticFiles(); // Serve static files (CSS, JS) for Identity UI
    app.UseRouting(); // Must be before UseIdentityServer and UseAuthorization
    app.UseIdentityServer(); // Add IdentityServer endpoints
    app.UseAuthorization(); // IMPORTANT: Authorization comes AFTER UseIdentityServer

    // Map endpoints
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages(); // Map scaffolded Identity Pages
    
    // Seed data just before running (ensures pipeline is mostly built)
    // Still only in Development environment
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Ensuring database is seeded...");
        // Uncomment seeding again
        SeedData.EnsureSeedDataAsync(app).GetAwaiter().GetResult();
        Log.Information("Seeding completed.");
    }
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "IdentityService API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// =============================================================================
// Static Configuration for IdentityServer (for development/testing)
// =============================================================================
public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new List<ApiScope>
        {
            // Define scopes that your APIs will protect
            new ApiScope("ecommerce.api", "Full access to Ecommerce API") 
            // Add more scopes as needed, e.g., "product.read", "product.write"
        };

    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        // Client configuration requires the Blazor app's URL and the Gateway's URL
        string blazorAppUrl = configuration["ClientUrls:BlazorAppUrl"] ?? "https://localhost:7001"; // Default if not set
        string gatewayUrl = configuration["ClientUrls:GatewayUrl"] ?? "https://localhost:7200"; // Default if not set
        
        return new List<Client>
        {
            // Blazor WASM client using Authorization Code Flow with PKCE
            new Client
            {
                ClientId = "blazor_wasm",
                ClientName = "Blazor WebAssembly Client",
                AllowedGrantTypes = GrantTypes.Code, // Authorization Code Flow
                RequirePkce = true,
                RequireClientSecret = false, // No secret needed for public client
                
                // Where the client is allowed to request token redirection
                RedirectUris = { $"{blazorAppUrl}/authentication/login-callback" },
                // Where the client is allowed to logout from
                PostLogoutRedirectUris = { $"{blazorAppUrl}/authentication/logout-callback" },
                // Origins allowed to connect
                AllowedCorsOrigins = { blazorAppUrl },

                AllowedScopes = { 
                    IdentityServerConstants.StandardScopes.OpenId, 
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "ecommerce.api" // Allow access to our API scope
                },

                // Ensure user claims like email are included in the ID token
                AlwaysIncludeUserClaimsInIdToken = true, 

                AllowOfflineAccess = true, // Enable refresh tokens if needed
                RefreshTokenUsage = TokenUsage.ReUse
            },

             // API Gateway Client (for service-to-service communication, optional for now)
             // If the gateway needs to call services on behalf of itself
             /*
             new Client
             {
                 ClientId = "api_gateway",
                 ClientName = "API Gateway",
                 AllowedGrantTypes = GrantTypes.ClientCredentials,
                 ClientSecrets = { new Secret("your_gateway_secret".Sha256()) }, // Store securely!
                 AllowedScopes = { "ecommerce.api" } 
             }
             */
        };
    }
}
