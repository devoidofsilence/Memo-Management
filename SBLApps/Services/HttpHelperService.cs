using Newtonsoft.Json;
using System.Text;

namespace SBLApps.Services
{
    public class HttpHelperService
    {
        // Inject IConfiguration into your class constructor
        private readonly IConfiguration _configuration;
        public HttpHelperService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public async Task<string> PostHandler(string serializedPostObject, string endpointUrl)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"{endpointUrl}");

                var content = new StringContent(serializedPostObject, Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
