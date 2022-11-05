using System.Net.Http.Headers;

namespace LSCProgram.RESTClient
{
    public class Client
    {
        public static HttpClient httpClient { get; set; }
        public Client(string baseUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
