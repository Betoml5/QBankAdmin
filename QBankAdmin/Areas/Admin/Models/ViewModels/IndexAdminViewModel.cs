using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class IndexAdminViewModel
    {
        public IEnumerable<CajaDTO>? Cajas { get; set; }
        public IEnumerable<UsuarioDTO>? Usuarios { get; set; }

        public int TotalTurnos { get; set; }
        public int TurnosCompletados { get; set; }
        public int TurnosCancelados { get; set; }

    }
}
