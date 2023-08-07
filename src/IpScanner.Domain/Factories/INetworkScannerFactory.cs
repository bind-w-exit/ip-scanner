using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public interface INetworkScannerFactory
    {
        NetworkScanner CreateBasedOnIpRange(IpRange range);
    }
}
