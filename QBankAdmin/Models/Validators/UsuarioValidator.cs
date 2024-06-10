using FluentValidation;
using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using System.Linq;

namespace QBankAdmin.Models.Validators
{
    public class UsuarioValidator:AbstractValidator<UsuarioDTO>
    {
        static string remoteAddress = "https://qbank.websitos256.com/api/";
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
        };


        public UsuarioValidator()
        {
            RuleFor(x => x.CorreoElectronico).EmailAddress().WithMessage("Ingrese una dirección de correo válida.");
            RuleFor(x => x.CorreoElectronico).MustAsync(ValidarCorreoExistente).WithMessage("El correo electrónico ya se encuentra registrado.");
            RuleFor(x => x.Contrasena).NotEmpty().WithMessage("Debe ingresar una contraseña.");
            RuleFor(x => x.Contrasena).MinimumLength(8).WithMessage("La contraseña debe contener un mínimo de 8 caracteres.");
            RuleFor(x => x.Rol).NotEmpty().WithMessage("Debe seleccionar un rol para el usuario.");
        }


        private async Task<bool> ValidarCorreoExistente(string correo, CancellationToken cancellationToken)
        {
            if(correo != null)
            {
                HttpResponseMessage response = await client.GetAsync("Usuario");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var listadousuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(data);
                    if(listadousuarios.Any(u => u.CorreoElectronico == correo))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
