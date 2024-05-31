using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;

namespace QBankAdmin.Models.ViewModels
{
    public class CajaService
    {

        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
        };

        public async Task<IEnumerable<CajaDTO>>? Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("caja");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<CajaDTO>>(data);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return new List<CajaDTO>();
        }
    }
}
