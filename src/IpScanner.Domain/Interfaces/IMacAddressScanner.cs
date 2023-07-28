using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IMacAddressScanner
    {
        Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination);
    }
}
