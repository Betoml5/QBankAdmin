using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
        };

        CajaService cajaService = new();

        public IActionResult Index()
        {
            var cajas = cajaService.Get().Result;

            IndexAdminViewModel model = new()
            {
                Cajas = cajas
            };
            return View(model);
        }
    }
}
