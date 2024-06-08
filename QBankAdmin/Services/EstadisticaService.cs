using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Services
{
    public class EstadisticaService
    {
         static string remoteAddress = "https://qbank.websitos256.com/api/";

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
        };

        public async Task<IEnumerable<EstadisticaDTO>> GetAllEstadisticas()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("estadisticas");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<EstadisticaDTO>>(data);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return new List<EstadisticaDTO>();
        }

        public async Task<EstadisticaDTO> EstadisticasHoy()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("estadisticas/today");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<EstadisticaDTO>(data);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }




        public async Task<bool> EnviarEstadisticas()
        {
            try
            {
                var contenido = new StringContent(string.Empty);
                HttpResponseMessage response = await client.PostAsync("estadisticas", contenido);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

    }
}
