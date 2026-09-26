using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MiniClinik.Web.Models;
using MiniClinik.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MiniClinik.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel { Role = "patient" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await authService.LoginAsync(model.Email, model.Password, model.Role);

            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email, password, or role.");
                return View(model);
            }

            // JWT token ke andar se claims nikalna (role, userId, email)
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = new List<Claim>(jwtToken.Claims)
            {
                new Claim("access_token", token) // raw JWT bhi cookie mein chhupa ke rakh rahe hain — future API calls ke liye
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // ab browser ko auth cookie bhej rahe hain
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Dashboard", model.Role); // agle step mein banayenge
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
