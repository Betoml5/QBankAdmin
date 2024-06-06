namespace QBankAdmin.Models.Dtos
{
	public class EstadisticaDTO
	{
		public int TurnosCancelados { get; set; }
		public int TurnosAtendidos { get; set; }
		public int TurnosPendientes { get; set; }
		public int TurnosEnProgreso { get; set; }
		public double TiempoPromedioEspera {  get; set; }
		public double TiempoPromedioDeAtencion {  get; set; }
    }
}
