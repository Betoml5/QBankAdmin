using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {


        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
        };

        CajaService cajaService = new();
        TurnoService turnoService = new();
        UsuarioService usuarioService = new();
        ConfiguracionService configuracionService = new();

        public IActionResult Index()
        {
            var cajas = cajaService.Get().Result;
            var usuarios = usuarioService.Get().Result;
            var estadisticas = turnoService.Estadisticas().Result;

            IndexAdminViewModel model = new()
            {
                Cajas = cajas,
                Usuarios = usuarios,
                Estadisticas = estadisticas
                
            };
            return View(model);
        }

        public IActionResult Estadisticas()
        {
            var estadisticas = turnoService.Estadisticas().Result;


            EstadisticasViewModel vm = new()
            {
                EstadisticaActual = estadisticas
            };
            return View(vm);
        }

        public async Task<IActionResult> EnviarEstadistica()
        {
            bool estadisticasenviadas = await turnoService.EnviarEstadisticas();

            if (estadisticasenviadas)
            {
                return RedirectToAction("Estadisticas", "Home", new { area = "Admin" });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
