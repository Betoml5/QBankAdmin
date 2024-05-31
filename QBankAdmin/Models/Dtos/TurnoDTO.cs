namespace QBankAdmin.Models.Dtos
{
    public class TurnoDTO
    {
        public int Id { get; set; }

        public int IdCaja { get; set; }

        public int TiempoEspera { get; set; }
        public string Caja { get; set; } = null!;

        public string CodigoTurno { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }


        public string Estado { get; set; } = null!;
    }
}
