using Microsoft.AspNetCore.Mvc;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Models.ViewModels;
using QBankAdmin.Services;

namespace QBankAdmin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuarioController : Controller
    {
        UsuarioService usuarioService = new();

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
        };

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(UsuarioDTO usuario)
        {
            if (usuario != null)
            {
                var usuariocreado = await usuarioService.Create(usuario);
                if (usuariocreado != null)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            return View(usuario);
        }



        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await usuarioService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioDTO usuario)
        {
            bool actualizacion = await usuarioService.Update(usuario);
            if (actualizacion)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View(usuario);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await usuarioService.GetById(id);

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(UsuarioDTO usuario)
        {
            bool eliminado = await usuarioService.Delete(usuario.Id);
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
