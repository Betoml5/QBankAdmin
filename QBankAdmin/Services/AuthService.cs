using Newtonsoft.Json;
using QBankAdmin.Models.Dtos;
using System.Text;

namespace QBankAdmin.Services
{
    public class AuthService
    {

        HttpClient client = new HttpClient();
        static string localAddress = "https://localhost:5001/api/";
        static string remoteAddress = "https://qbank.websitos256.com/api/";
        public AuthService() { 
        
                client.BaseAddress = new Uri(remoteAddress);

        }

        public async Task<CajaDTO> Login(string nombreUsuario, string contrasena)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { nombreUsuario, contrasena }), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("auth", content);
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

        public async Task<UsuarioDTO> LoginAdmin(string nombreUsuario, string contrasena)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { nombreUsuario, contrasena }), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("auth/admin", content);
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

    }
}
