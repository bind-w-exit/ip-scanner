using System.Linq;
using System.Net;

namespace IpScanner.Domain.Extensions
{
    internal static class IPAddressExtensions
    {
        public static int GetHostId(this IPAddress ipAddress)
        {
            return int.Parse(ipAddress.ToString().Split('.').Last());
        }

        public static IPAddress GetLocalIPAddress(this int hostId)
        {
            return IPAddress.Parse($"192.168.0.{hostId}");
        }
    }
}
