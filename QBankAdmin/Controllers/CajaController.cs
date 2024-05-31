using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Services;

namespace QBankAdmin.Controllers
{
    public class CajaController : Controller
    {
        CajaService cajaService = new();

        public IActionResult VerAgregar()
        {
            return View();
        }

        public async Task<IActionResult> Create(CajaDTO caja)
        {
            var cajacreada = await cajaService.Create(caja);
            if (cajacreada != null)
            {
            return RedirectToAction(nameof(Index));
            }
            return View(caja);
        }

        public async Task<IActionResult> VerEditar(int id)
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

        public async Task<IActionResult> Editar(CajaDTO caja)
        {
            bool actualizacion = await cajaService.Update(caja);
            if (actualizacion)
            {
              return RedirectToAction(nameof(Index));
            }

            return View(caja);
        }

        public async Task<IActionResult> VerEliminar(int id)
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

        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await cajaService.Delete(id);
            if (eliminado)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
