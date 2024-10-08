﻿namespace QBankAdmin.Models.Dtos
{
	public class EstadisticaDTO
	{
		public DateTime Fecha { get; set; }
		public int TurnosCancelados { get; set; }
		public int TurnosAtendidos { get; set; }
		public int TurnosPendientes { get; set; }
		public int TurnosEnProgreso { get; set; }
		public double TiempoPromedioDeEspera {  get; set; }
		public double TiempoPromedioDeAtencion {  get; set; }
    }
}
