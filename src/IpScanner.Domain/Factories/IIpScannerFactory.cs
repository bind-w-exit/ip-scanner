using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public interface IIpScannerFactory
    {
        Models.IpScanner CreateBasedOnIpRange(IpRange range);
    }
}
