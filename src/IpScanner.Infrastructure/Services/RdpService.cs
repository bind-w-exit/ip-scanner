using System.Diagnostics;
using IpScanner.Infrastructure.Settings;

namespace IpScanner.Infrastructure.Services
{
    public class RdpService : IRdpService
    {
        public void Connect(RdpConfiguration configuration)
        {
            string arguments = $"/v:{configuration.IpAddress}";
            Process.Start("mstsc.exe", arguments);
        }
    }
}
