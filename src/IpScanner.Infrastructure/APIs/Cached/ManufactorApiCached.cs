using IpScanner.Domain.Interfaces;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.APIs.Cached
{
    public class ManufactorApiCached : IManufactorRepository
    {
        private readonly IManufactorRepository _manufactorReceiver;
        private readonly Dictionary<PhysicalAddress, string> _cache;

        public ManufactorApiCached(IManufactorRepository manufactorReceiver)
        {
            _cache = new Dictionary<PhysicalAddress, string>();
            _manufactorReceiver = manufactorReceiver;
        }

        public async Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress)
        {
            if (_cache.ContainsKey(macAddress))
            {
                return _cache[macAddress];
            }

            var result = await _manufactorReceiver.GetManufacturerOrEmptyStringAsync(macAddress);
            _cache.Add(macAddress, result);

            return result;
        }
    }
}
