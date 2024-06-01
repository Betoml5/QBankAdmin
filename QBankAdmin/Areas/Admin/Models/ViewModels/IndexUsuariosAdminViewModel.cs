using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Areas.Admin.Models.ViewModels
{
    public class IndexUsuariosAdminViewModel
    {
        public IEnumerable<UsuarioDTO> Usuarios { get; set; }
    }
}
