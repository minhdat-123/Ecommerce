using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace IdentityService.Application.Interfaces
{
    /// <summary>
    /// Application wrapper for Duende IdentityServer's IProfileService 
    /// to maintain clean architecture separation
    /// </summary>
    public interface IIdentityProfileService : IProfileService
    {
        // Inherits GetProfileDataAsync and IsActiveAsync from Duende's IProfileService
    }
}
