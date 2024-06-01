using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using System.Text;

namespace QBankAdmin.Services
{
    public class UsuarioService
    {
        HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://qbank.websitos256.com/api/")
        };

        public async Task<IEnumerable<UsuarioDTO>>? Get()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("Usuario");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<UsuarioDTO>>(data);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return new List<UsuarioDTO>();
        }

        public async Task<UsuarioDTO> Create(UsuarioDTO usuario)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("Usuario", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UsuarioDTO>(data);
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
                HttpResponseMessage response = await client.DeleteAsync($"Usuario/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        public async Task<bool> Update(UsuarioDTO usuario)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"usuario/", usuario);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public async Task<UsuarioDTO> GetById(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"usuario/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UsuarioDTO>(data);
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
