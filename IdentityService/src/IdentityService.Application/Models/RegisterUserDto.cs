using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.Models
{
    /// <summary>
    /// DTO for user registration
    /// </summary>
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}
