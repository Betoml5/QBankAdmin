using Microsoft.AspNetCore.Mvc;

namespace QBankAdmin.Areas.Admin.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
