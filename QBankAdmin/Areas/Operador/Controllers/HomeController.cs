using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Operador.Models;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Operador.Controllers
{
    [Area("Operador")]
    public class HomeController : Controller
    {

        TurnoService turnoService = new();
        public IActionResult Index()
        {
            var turnos = turnoService.Get().Result;

            IndexViewModel vm = new()
            {
                Turnos = turnos
            };

            return View(vm);
        }
    }
}
