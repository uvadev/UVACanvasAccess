using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model;

namespace UVACanvasAccess {
    public class Api {

        private readonly HttpClient client;
        
        public Api(string token, string baseUrl) {
            client = new HttpClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        }

        private Task<HttpResponseMessage> RawGetListUsers(string searchTerm, string accountId = "self") {
            return client.GetAsync("accounts/" + accountId + "/users?search_term=" + searchTerm);
        }

        public async Task<List<User>> GetListUsers(string searchTerm, string accountId = "self") {
            var response = await RawGetListUsers(searchTerm, accountId);
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(responseStr);
        }
    }
}