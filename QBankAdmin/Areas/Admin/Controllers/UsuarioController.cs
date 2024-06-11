using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using QBankAdmin.Areas.Admin.Models.ViewModels;
using QBankAdmin.Models.Dtos;
using QBankAdmin.Models.Validators;
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
            UsuariosViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(UsuariosViewModel vm)
        {
            UsuarioValidator validador = new();
            var resultado = await validador.ValidateAsync(vm.Usuario);
                if (resultado.IsValid)
                {
                    var usuariocreado = await usuarioService.Create(vm.Usuario);
                    if (usuariocreado != null)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                else
                {
                   vm.Errores = resultado.Errors.Select(x => x.ErrorMessage).ToList();
                   return View(vm);
                }
            return View();
        }



        public async Task<IActionResult> Editar(int id)
        {
            UsuariosViewModel vm = new();
            var usuario = await usuarioService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                vm.Usuario = usuario;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuariosViewModel vm)
        {
            UsuarioValidator validador = new();
            var resultado = await validador.ValidateAsync(vm.Usuario);
            if (resultado.IsValid)
            {
                bool actualizacion = await usuarioService.Update(vm.Usuario);
                if (actualizacion)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            else
            {
                vm.Errores = resultado.Errors.Select(x => x.ErrorMessage).ToList();
                return View(vm);
            }
            return View();
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
