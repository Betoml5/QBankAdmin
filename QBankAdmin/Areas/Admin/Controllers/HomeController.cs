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

        public IActionResult Index()
        {
            var cajas = cajaService.Get().Result;
            var usuarios = usuarioService.Get().Result;

            var turnos = turnoService.Get().Result.Where(x => x.FechaCreacion.Date == DateTime.Today);

            var totalturnos = turnoService.GetAll().Result.Where(x=>x.FechaCreacion.Date == DateTime.Today).Count();
            var turnoscompletados = turnoService.GetAll().Result.Where(x => x.Estado == "completado" && x.FechaCreacion.Date == DateTime.Today).Count();
            var turnoscancelados = turnoService.GetAll().Result.Where(x => x.Estado == "cancelado" && x.FechaCreacion.Date == DateTime.Today).Count();

            //TimeSpan? sumastiempoatencion;

            //foreach (var item in turnos)
            //{
            //    sumastiempoatencion = item.FechaFinalizacion - item.FechaAtencion;
            //}





            IndexAdminViewModel model = new()
            {
                Cajas = cajas,
                Usuarios = usuarios,
                TotalTurnos = totalturnos,
                TurnosCancelados = turnoscompletados,
                TurnosCompletados = turnoscancelados
            };
            return View(model);
        }
    }
}
