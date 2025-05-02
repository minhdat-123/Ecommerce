using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using IdentityService.Application;
using IdentityService.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;
using Serilog;
using IdentityService.API;
using IdentityService.API.Areas.Identity.Services;

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

    // Add application layer services
    builder.Services.AddApplication();

    // Add infrastructure layer services (includes DbContext and Identity)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Register Identity UI Service to bridge Razor Pages with application layer
    builder.Services.AddScoped<IdentityUIService>();
    
    // Register IEmailSender for Identity UI registration flow
    builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, IdentityService.API.Services.EmailSender>();

    // Configure cookie and authentication options
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.Cookie.Name = "IdentityService.Cookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

    // Configure authentication defaults
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    });

    // Configure IdentityServer
    builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            
            // Add user interaction configuration
            options.UserInteraction = new Duende.IdentityServer.Configuration.UserInteractionOptions
            {
                LoginUrl = "/Identity/Account/Login",
                LogoutUrl = "/Identity/Account/Logout",
                ErrorUrl = "/Identity/Error"
            };
        })
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiScopes(Config.GetApiScopes())
        .AddInMemoryClients(Config.GetClients(builder.Configuration))
        .AddAspNetIdentity<ApplicationUser>();
    
    // Note: IProfileService is registered by the Application layer
    // in IdentityService.Application.DependencyInjection.cs
    
    // Add services to the container
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    
    // Add API controllers with JSON serialization options
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

    // Add API versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    });

    // Add CORS policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("default", policy =>
        {
            policy.WithOrigins(
                builder.Configuration["ClientUrls:BlazorAppUrl"] ?? "https://localhost:7001",
                builder.Configuration["ClientUrls:GatewayUrl"] ?? "https://localhost:7200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    });
    
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
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("default");
    app.UseIdentityServer();
    app.UseAuthorization();

    // Map endpoints
    // First map controller routes for traditional MVC controllers 
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
        
    // Then map Razor Pages for Identity UI
    app.MapRazorPages();
    
    // Finally map API controllers 
    app.MapControllers();
    
    // Seed data just before running (ensures pipeline is mostly built)
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Ensuring database is seeded...");
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
            new ApiScope("productservice.api", "Full access to ProductService API") 
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
                    "productservice.api" // Allow access to our API scope
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
                 AllowedScopes = { "productservice.api" } 
             }
             */
        };
    }
}
