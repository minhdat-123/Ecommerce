using Ecommerce.Application;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Extensions;
using Ecommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration --- 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Removed JWT Key/Issuer/Audience variables as authentication is now handled by IdentityServer

// --- Service Registration --- 

// Add Infrastructure, Application, Elasticsearch
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();
builder.Services.AddElasticsearch(builder.Configuration);

// Remove local Identity setup - Handled by IdentityService
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure validation against IdentityService
    options.Authority = "https://localhost:7273"; // URL of your IdentityService
    options.Audience = "ecommerce.api"; // The API scope name defined in IdentityService

    options.RequireHttpsMetadata = false; // Set to true in production environments
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true, // Ensure the token is intended for this API
        ValidateIssuer = true,   // Ensure the token came from the correct authority
        // IssuerSigningKey validation is handled automatically via the discovery endpoint
        NameClaimType = "name", // Optional: Map 'name' claim
        RoleClaimType = "role"  // Optional: Map 'role' claim
    };
});

// Add Authorization (can add policies later if needed)
builder.Services.AddAuthorization();

// Add Controllers and CORS
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    // Keeping existing CORS policy, ensure Blazor origin is allowed
    options.AddPolicy("AllowLocalhost4200", 
        builder => builder.WithOrigins("http://localhost:4200", "https://localhost:7235") // Make sure Blazor app URL is here
                          .AllowAnyHeader()
                          .AllowAnyMethod());
    // Adding a default permissive policy for simplicity during dev, review for production
     options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Add Swagger with Auth configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1"
    });

    // Add JWT Authentication support to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field (e.g., Bearer eyJ...)",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Migrations and seeding are now handled via the MigrationController endpoint
// // Apply migrations at startup
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.Migrate();
// }

// Seeding is now handled via the MigrationController endpoint
// // Seed the database
// using (var scope = app.Services.CreateScope())
// {
//     var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
//     await seeder.SeedAsync();
// }

// --- Middleware Pipeline --- 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
    });
    app.UseDeveloperExceptionPage(); // Good for development
}
else
{
    app.UseExceptionHandler("/Error"); // Configure proper error handling for production
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(); // Use the default policy, or specify the named one if preferred: app.UseCors("AllowLocalhost4200");

// IMPORTANT: Authentication MUST come before Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
