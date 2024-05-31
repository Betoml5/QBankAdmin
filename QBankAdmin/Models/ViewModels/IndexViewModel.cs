using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<TurnoDTO>? Turnos { get; set; }
        public IEnumerable<CajaDTO>? Cajas { get; set; }
    }
}
