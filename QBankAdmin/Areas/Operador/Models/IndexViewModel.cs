using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Operador.Models
{
    public class IndexViewModel
    {
        public IEnumerable<TurnoDTO>? Turnos { get; set; }
    }
}
