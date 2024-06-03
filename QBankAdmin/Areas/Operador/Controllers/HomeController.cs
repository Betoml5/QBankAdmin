using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Operador.Models;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Operador.Controllers
{
    [Area("Operador")]
    [Authorize(Roles = "Operador")]
    public class HomeController : Controller
    {

        TurnoService turnoService = new();
        CajaService cajaService = new();
        public IActionResult Index()
        {
            var turnos = turnoService.Get().Result;
            var cajas = cajaService.Get().Result.Where(caja => caja.Estado == "inactiva");

            IndexViewModel vm = new()
            {
                Turnos = turnos,
                Cajas = cajas
            };

            return View(vm);
        }
    }
}
