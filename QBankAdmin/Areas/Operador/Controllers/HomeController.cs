using Microsoft.AspNetCore.Mvc;

namespace QBankAdmin.Areas.Operador.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
