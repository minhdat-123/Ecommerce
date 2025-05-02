using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Models
{
    /// <summary>
    /// DTO for user login
    /// </summary>
    public class LoginUserDto
    {
        [Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
