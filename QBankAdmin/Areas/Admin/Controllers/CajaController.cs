using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Models.Validators;
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
            CajasViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(CajasViewModel vm)
        {
            CajaValidator validador = new();
            var resultado = await validador.ValidateAsync(vm.Caja);
            if (resultado.IsValid)
            {
                var cajacreada = await cajaService.Create(vm.Caja);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                vm.Errores = resultado.Errors.Select(x => x.ErrorMessage).ToList();
            }
            return View(vm);
        }

        public async Task<IActionResult> Editar(int id)
        {
            CajasViewModel vm = new();
            var caja = await cajaService.GetById(id);
            if (caja == null)
            {
                return NotFound();
            }
            else
            {
                vm.Caja = caja;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CajasViewModel vm)
        {
            CajaValidator validador = new();
            var resultado = await validador.ValidateAsync(vm.Caja);
            if (resultado.IsValid)
            {
                bool actualizacion = await cajaService.Update(vm.Caja);
                if (actualizacion)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            else
            {
                vm.Errores = resultado.Errors.Select(x => x.ErrorMessage).ToList();
            }
            return View(vm);
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
