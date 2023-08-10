using IpScanner.Infrastructure.Settings;

namespace IpScanner.Infrastructure.Services
{
    public interface IRdpService
    {
        void Connect(RdpConfiguration configuration);
    }
}
