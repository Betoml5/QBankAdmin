using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;
using System.Security.Claims;

namespace QBankAdmin.Controllers
{
    public class LoginController : Controller
    {

        AuthService authService = new AuthService();
        public IActionResult Index(IndexLoginViewModel vm)
        {
            var user = authService.Login(vm.Usuario, vm.Contrasena).Result;
            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new ("Id", user.Id.ToString()),
                    new (ClaimTypes.Name, user.Nombre),
                    new (ClaimTypes.Role, user.Rol),
                    new (ClaimTypes.Email, user.CorreoElectronico)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties()
                {
                    IsPersistent = true,
                });


                if (user.Rol == "Admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (user.Rol == "Operador")
                {
                    return RedirectToAction("Index", "Home", new {area="Operador"});
                }
            }


            return View(vm);
        }
    }
}
