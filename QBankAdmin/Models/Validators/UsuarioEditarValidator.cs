using FluentValidation;
using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Models.Validators
{
    public class UsuarioEditarValidator:AbstractValidator<UsuarioDTO>
    {
        static string remoteAddress = "https://qbank.websitos256.com/api/";
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
        };


        public UsuarioEditarValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Ingrese un nombre de usuario.");
            RuleFor(x => x.Nombre).MinimumLength(3).WithMessage("El nombre de usuario debe contener un mínimo de 3 caracteres.");

            RuleFor(x => x.CorreoElectronico).EmailAddress().WithMessage("Ingrese una dirección de correo electrónico válida.");
            RuleFor(x => x.CorreoElectronico).NotEmpty().WithMessage("Ingrese una direccion de correo electrónico.");
            RuleFor(x => x.CorreoElectronico).MustAsync((usuario, correo, cancellationToken) => ValidarCorreoExistente(correo, usuario.Id, cancellationToken)).WithMessage("El correo electrónico ya se encuentra registrado.");

            RuleFor(x => x.RolId).NotEmpty().WithMessage("Debe seleccionar un rol para el usuario.");
        }


        private async Task<bool> ValidarCorreoExistente(string correo, int id, CancellationToken cancellationToken)
        {
            if (correo != null)
            {
                HttpResponseMessage response = await client.GetAsync("Usuario");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var listadousuarios = JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(data);
                    if (listadousuarios != null)
                    {
                        if (listadousuarios.Any(u => u.CorreoElectronico == correo && u.Id != id))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
