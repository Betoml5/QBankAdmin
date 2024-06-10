using FluentValidation;
using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Models.Validators
{
    public class CajaValidator:AbstractValidator<CajaDTO>
    {
        public CajaValidator()
        {
            var estadosValidos = new List<string> { "completado", "pendiente", "cancelado", "en espera" };

            RuleFor(x => x.NumeroCaja).NotEmpty().WithMessage("Debe ingresar el número de caja.");
            RuleFor(x=>x.Estado).Must(ValidarEstado)
            .WithMessage("El estado debe ser uno de los siguientes: completado, pendiente, cancelado, en espera");
        }

        private bool ValidarEstado(string estado)
        {
            if (estado != null)
            {
                if(estado == "completado" || estado == "pendiente" || estado == "cancelado" || estado == "atendiendo")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
