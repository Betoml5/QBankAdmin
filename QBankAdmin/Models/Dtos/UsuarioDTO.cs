namespace QBankAdmin.Models.Dtos
{
    public class UsuarioDTO
    {

        public int Id { get; set; }

        public string Nombre { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Contrasena { get; set; } = null!;

        public int RolId { get; set; }

        public string Rol { get; set; } = null!;

    }


}
