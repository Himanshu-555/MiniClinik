using MiniClinik.Web.Models;
using System.Text;
using System.Text.Json;

namespace MiniClinik.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;

        public AuthService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<string?> LoginAsync(string email, string password, string role)
        {
            string endpoint = role switch
            {
                "patient" => "api/auth/login",
                "Doctor" => "api/auth/doctor/login",
                "Admin" => "api/auth/admin/login",
                _ => throw new ArgumentException("Invalid role")
            };
            var payload = new { email, password };
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return null; // API ne Unauthorized ya error diya

            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseBody);

            // API "{ token: '...' }" return karta hai (AuthController dekho)
            return doc.RootElement.GetProperty("token").GetString();
        }
    }
}
