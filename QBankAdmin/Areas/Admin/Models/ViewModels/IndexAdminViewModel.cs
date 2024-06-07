using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class IndexAdminViewModel
    {
        public IEnumerable<CajaDTO>? Cajas { get; set; }
        public IEnumerable<UsuarioDTO>? Usuarios { get; set; }
        
        public EstadisticaDTO Estadisticas { get; set;}
    }
}
