using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGatewayMock
{
   public class LoyaltyProgramClient
    {
        private readonly HttpClient _httpClient;

        public LoyaltyProgramClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterUser(string name)
        {
            var user = new { name, Settings = new { } };
            return await _httpClient.PostAsync("/users/", CreateBody(user));
        }

        public async Task<HttpResponseMessage> UpdateUser(dynamic user) => await _httpClient.PutAsync($"/users/{user.Id}", CreateBody(user));

        public async Task<HttpResponseMessage> QueryUser(string arg) => await _httpClient.GetAsync($"/users/{int.Parse(arg)}");

        private static StringContent CreateBody(object user) =>
            new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");


    }
}
