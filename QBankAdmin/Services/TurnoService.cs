using QBankAdmin.Models.Dtos;
using Newtonsoft.Json;
namespace QBankAdmin.Services
{
    public class TurnoService
    {

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
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
