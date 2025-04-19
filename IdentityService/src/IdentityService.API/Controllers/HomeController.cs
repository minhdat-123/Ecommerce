using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Redirect to the discovery document for IdentityServer
            return Redirect("~/.well-known/openid-configuration");
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return RedirectToPage("/Error", new { area = "Identity" });
        }
    }
}
