using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class CajasViewModel
    {
        public CajaDTO Caja { get; set; } = null!;
        public List<string>? Errores { get; set; }
    }
}
