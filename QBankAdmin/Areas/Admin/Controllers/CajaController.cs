using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CajaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        CajaService cajaService = new();

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(CajaDTO caja)
        {
            if(caja != null)
            {
                var cajacreada = await cajaService.Create(caja);
                if (cajacreada != null)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View(caja);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var caja = await cajaService.GetById(id);
            if (caja == null)
            {
                return NotFound();
            }
            else
            {
                return View(caja);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CajaDTO caja)
        {
            bool actualizacion = await cajaService.Update(caja);
            if (actualizacion)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View(caja);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var caja = await cajaService.GetById(id);

            if (caja == null)
            {
                return NotFound();
            }
            else
            {
                return View(caja);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(CajaDTO dto)
        {
            bool eliminado = await cajaService.Delete(dto.Id);
            if (eliminado)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                return NotFound();
            }

        }
    }
}
