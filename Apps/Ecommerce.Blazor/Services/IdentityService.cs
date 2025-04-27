using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Blazor.Services
{
    /// <summary>
    /// Implementation of IIdentityService that communicates with the Identity Service
    /// through the API Gateway
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IdentityService> _logger;
        private readonly ApiSettings _apiSettings;

        public IdentityService(
            IHttpClientFactory httpClientFactory,
            ILogger<IdentityService> logger,
            ApiSettings apiSettings)
        {
            // Use the dedicated IdentityAPI client that's configured to go through the gateway
            _httpClient = httpClientFactory.CreateClient("IdentityAPI");
            _logger = logger;
            _apiSettings = apiSettings;
        }

        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            try
            {
                // Uses the /api/identity/users/me endpoint that returns the current user's profile
                return await _httpClient.GetFromJsonAsync<UserProfileDto>("users/me");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return null;
            }
        }

        public async Task<UserProfileDto> GetUserByIdAsync(string userId)
        {
            try
            {
                // Uses the /api/identity/users/{id} endpoint
                return await _httpClient.GetFromJsonAsync<UserProfileDto>($"users/{userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUsersAsync()
        {
            try
            {
                // Uses the /api/identity/users endpoint (admin only)
                return await _httpClient.GetFromJsonAsync<IEnumerable<UserProfileDto>>("users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return new List<UserProfileDto>();
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            try
            {
                // Uses the /api/identity/users/{userId}/roles endpoint
                return await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"users/{userId}/roles");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user {UserId}", userId);
                return new List<string>();
            }
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfileDto user)
        {
            try
            {
                // Uses the /api/identity/users/{id} endpoint with PUT method
                var response = await _httpClient.PutAsJsonAsync($"users/{user.Id}", user);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile for {UserId}", user.Id);
                return false;
            }
        }
    }
}
