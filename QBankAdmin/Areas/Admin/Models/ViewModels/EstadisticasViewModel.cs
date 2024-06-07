using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class EstadisticasViewModel
    {
        public EstadisticaDTO EstadisticaActual { get; set;}

        public IEnumerable<EstadisticaDTO>? HistorialEstadisticas { get; set;}
    }
}
