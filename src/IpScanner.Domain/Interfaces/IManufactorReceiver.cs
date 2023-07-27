using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IManufactorReceiver : IDisposable
    {
        Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress);
    }
}
