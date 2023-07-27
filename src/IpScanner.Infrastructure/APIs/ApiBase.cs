using System;
using System.Net;
using System.Net.Http;
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
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<T>();
        }

        public virtual void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
