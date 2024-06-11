using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class UsuariosViewModel
    {
        public UsuarioDTO Usuario { get; set; } = null!;
        public List<string>? Errores { get; set; }
    }
}
