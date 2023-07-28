using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.APIs
{
    public abstract class ApiBase
    {
        private readonly HttpClient _httpClient;

        protected ApiBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T> GetAsync<T>(Uri uri)
        {
            string content = await GetAsStringAsync(uri);
            return JsonSerializer.Deserialize<T>(content);
        }

        protected async Task<string> GetAsStringAsync(Uri uri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
