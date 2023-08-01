using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public interface INetworkScannerFactory
    {
        Models.NetworkScanner CreateBasedOnIpRange(IpRange range);
    }
}
