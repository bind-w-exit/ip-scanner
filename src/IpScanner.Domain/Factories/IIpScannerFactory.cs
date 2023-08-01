using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public interface IIpScannerFactory
    {
        Models.NetworkScanner CreateBasedOnIpRange(IpRange range);
    }
}
