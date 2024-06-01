using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;
using System.Security.Claims;

namespace QBankAdmin.Controllers
{
    public class LoginAdminController : Controller
    {

        AuthService authService = new AuthService();
        public IActionResult Index(IndexLoginViewModel vm)
        {
            var caja = authService.LoginAdmin(vm.Usuario, vm.Contrasena).Result;
            if (caja != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new ("Id", caja.Id.ToString()),
                    new (ClaimTypes.Name, caja.NombreUsuario),
                    new (ClaimTypes.Role, "Admin"),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties()
                {
                    IsPersistent = true,
                });

                return RedirectToAction("Index", "Home", new { area = "Admin" });

            }
            return View(vm);
        }
    }
}
