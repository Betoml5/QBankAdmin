namespace QBankAdmin.Models.Dtos
{
    public class CajaDTO
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string NumeroCaja { get; set; } = null!;

        public string? Estado { get; set; }
    }
}
