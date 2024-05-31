using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using System.Security.Policy;
using System.Text;

namespace QBankAdmin.Services
{
    public class CajaService
    {
        static string localAddress = "https://localhost:5001/api/";
        static string remoteAddress = "https://qbank.websitos256.com/turno";



        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(remoteAddress)
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

        public async Task<CajaDTO> Create(CajaDTO caja)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(caja), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("caja", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CajaDTO>(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        public async Task<bool> Delete(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"caja/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        public async Task<bool> Update(CajaDTO caja)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(caja), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"caja/{caja.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public async Task<CajaDTO> GetById(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"caja/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<CajaDTO>(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

    }
}
