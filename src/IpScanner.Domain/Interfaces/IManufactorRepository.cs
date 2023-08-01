using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IManufactorRepository
    {
        Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress);
    }
}
