using QBankAdmin.Models.Dtos;
using Newtonsoft.Json;
namespace QBankAdmin.Services
{
    public class TurnoService
    {
        static string localAddress = "https://localhost:5001/api/";

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(localAddress)
        };

        public async Task<IEnumerable<TurnoDTO>>? Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("turno");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<TurnoDTO>>(data);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return new List<TurnoDTO>();
        }

       
    }
}
