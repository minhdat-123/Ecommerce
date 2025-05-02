using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Services;

namespace IdentityService.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all application services
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            
            // Register our profile service both as our interface and Duende's interface
            services.AddScoped<IIdentityProfileService, CustomProfileService>();
            services.AddScoped<IProfileService>(sp => sp.GetRequiredService<IIdentityProfileService>());
            
            return services;
        }
    }
}
