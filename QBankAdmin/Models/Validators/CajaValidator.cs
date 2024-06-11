using FluentValidation;
using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Models.Validators
{
    public class CajaValidator:AbstractValidator<CajaDTO>
    {
        static string remoteAddress = "https://qbank.websitos256.com/api/";
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
        };

        public CajaValidator()
        {
            RuleFor(x => x.NumeroCaja).NotEmpty().WithMessage("Debe ingresar el número de caja.");
            RuleFor(x => x.NumeroCaja).Must(NumeroValido).WithMessage("Debe ingresar un número de caja válido.");
            RuleFor(x=>x.NumeroCaja).MustAsync(ValidarNumero).WithMessage("Ya se encuentra registrada dicha caja.");
        }

        private async Task<bool> ValidarNumero(string numero, CancellationToken cancellationToken)
        {
            if (numero != null)
            {
                HttpResponseMessage response = await client.GetAsync("caja");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var listadocajas = JsonConvert.DeserializeObject<IEnumerable<CajaDTO>>(data);
                    if (listadocajas != null)
                    {
                        if (listadocajas.Any(u => u.NumeroCaja == numero))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool NumeroValido(string numeroCaja)
        {
            if(numeroCaja != null)
            {
                return int.TryParse(numeroCaja, out _);
            }
            return true;
            
        }
    }
}
