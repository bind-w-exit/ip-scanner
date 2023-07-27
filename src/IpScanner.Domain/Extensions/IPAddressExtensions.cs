using System.Linq;
using System.Net;

namespace IpScanner.Domain.Extensions
{
    public static class IPAddressExtensions
    {
        public static int GetHostId(this IPAddress ipAddress)
        {
            return int.Parse(ipAddress.ToString().Split('.').Last());
        }

        public static string GetNetworkId(this IPAddress ip)
        {
            var ipParts = ip.ToString().Split('.');

            string networkId = string.Join(".", ipParts.Take(3)) + ".";
            return networkId;
        }
    }
}
