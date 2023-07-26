using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Domain.Interfaces
{
    public interface IMacAddressScanner
    {
        PhysicalAddress GetMacAddress(IPAddress ipAddress);
    }
}
