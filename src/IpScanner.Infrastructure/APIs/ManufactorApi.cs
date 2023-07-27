using IpScanner.Domain.Interfaces;
using IpScanner.Infrastructure.Extensions;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.APIs
{
    public class ManufactorApi : ApiBase, IManufactorReceiver
    {
        public ManufactorApi(HttpClient httpClient) : base(httpClient)
        { }

        public async Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress)
        {
            try
            {
                Uri uri = macAddress.GetUrlToFindManufacturer();
                return await GetAsStringAsync(uri);
            }
            catch (HttpRequestException)
            {
                return string.Empty;
            }
        }
    }
}
