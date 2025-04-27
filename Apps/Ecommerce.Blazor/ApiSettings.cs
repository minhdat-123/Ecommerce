namespace Ecommerce.Blazor
{
    /// <summary>
    /// Configuration settings for API endpoints
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Base URL for API calls
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Base URL for Identity API calls
        /// </summary>
        public string IdentityApiUrl { get; set; } = string.Empty;
    }
}