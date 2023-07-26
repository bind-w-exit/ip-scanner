using System.Net.NetworkInformation;

namespace IpScanner.Domain.Interfaces
{
    public interface IManufactorReceiver
    {
        string GetManufacturerOrEmptyString(PhysicalAddress macAddress);
    }
}
