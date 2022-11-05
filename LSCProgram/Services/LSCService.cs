using LSCProgram.RESTClient;
using LSC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace LSCProgram.Services
{
    public class LSCService
    {
        public async Task<string> GetToken()
        {
            using (HttpResponseMessage response = await Client.httpClient.GetAsync("api/token"))
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponse>();
                return apiResponse.Token;
                    
            }
        }
        public async Task<List<string>> GetCategories(string token)
        {
            using (HttpResponseMessage response = await Client.httpClient.GetAsync($"api/categories/{token}"))
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponseString>();
                return apiResponse.Data;
            }
        }
        public async Task<List<int>> GetMagazines(string token, string category)
        {
            using (HttpResponseMessage response = await Client.httpClient.GetAsync($"api/magazines/{token}/{category}/"))
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponseMagazine>();
                List<int> magazines = new List<int>();
                foreach (Magazine magazine in apiResponse.Data)
                {
                    magazines.Add(magazine.Id);
                }
                return magazines;
            }
        }
        public async Task<List<Subscriber>> GetSubscribers(string token)
        {
            using (HttpResponseMessage response = await Client.httpClient.GetAsync($"api/subscribers/{token}"))
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponseSubscriber>();
                return apiResponse.Data;
            }
        }
        public async Task<AnswerResponse> PostResult(string token, List<string> content)
        {
            var contentObject = new Dictionary<string, List<string>>
            {
                {
                    "subscribers", content
                },
            };

            string contentObjectSerialized = JsonConvert.SerializeObject(contentObject);
            using (HttpResponseMessage response = await Client.httpClient.PostAsync($"api/answer/{token}", new StringContent(contentObjectSerialized, Encoding.UTF8, "application/json")))
            {
                var apiResponse = await response.Content.ReadAsAsync<ApiResponseAnswerResponse>();
                return apiResponse.Data;
            }
        }
    }
}
