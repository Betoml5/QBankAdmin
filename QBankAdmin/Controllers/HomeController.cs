using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;

namespace QBankAdmin.Controllers
{
    public class HomeController : Controller
    {

        TurnoService turnoService = new();
        CajaService cajaService = new();

        public IActionResult Index()
        {

            var turnos = turnoService.Get().Result;
            var cajas = cajaService.Get().Result;

            IndexViewModel model = new()
            {
                Turnos = turnos,
                Cajas = cajas
            };

            return View(model);
        }
    }
}
