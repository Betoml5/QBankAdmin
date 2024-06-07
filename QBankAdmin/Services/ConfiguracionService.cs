using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using System.Text;

namespace QBankAdmin.Services
{
    public class ConfiguracionService
    {
        static string localAddress = "https://localhost:5002/api/";
        static string remoteAddress = "https://qbank.websitos256.com/api/";



        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
        };


        public async Task<ConfiguracionDTO>? Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("configuracion");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ConfiguracionDTO>(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new ConfiguracionDTO();
        }

        public async Task Update(ConfiguracionDTO configuracion)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(configuracion), Encoding.UTF8, "application/json"); 
                HttpResponseMessage response = await client.PutAsync("configuracion", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error al actualizar la configuración");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
