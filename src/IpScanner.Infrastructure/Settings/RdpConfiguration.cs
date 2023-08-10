using System.Net;

namespace IpScanner.Infrastructure.Settings
{
    public class RdpConfiguration
    {
        public RdpConfiguration(IPAddress ipAddress)
        {
            IpAddress = ipAddress;
        }

        public IPAddress IpAddress { get; }
    }
}
