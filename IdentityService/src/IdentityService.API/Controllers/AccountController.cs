using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;

namespace IdentityService.API.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // This controller handles redirects from /Account/Login to /Identity/Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Redirect to the ASP.NET Core Identity UI login page in the Areas folder
            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
        }

        [HttpGet]
        public IActionResult Logout(string returnUrl = null)
        {
            // Redirect to the ASP.NET Core Identity UI logout page in the Areas folder
            return RedirectToPage("/Account/Logout", new { area = "Identity", returnUrl });
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            // Redirect to the ASP.NET Core Identity UI access denied page in the Areas folder
            return RedirectToPage("/Account/AccessDenied", new { area = "Identity", returnUrl });
        }
    }
}
