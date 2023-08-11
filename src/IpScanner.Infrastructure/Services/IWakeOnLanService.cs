using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Services
{
    public interface IWakeOnLanService
    {
        Task SendPacketAsync(PhysicalAddress macAddress, IPAddress ipAddress, int port = 9);
    }
}
