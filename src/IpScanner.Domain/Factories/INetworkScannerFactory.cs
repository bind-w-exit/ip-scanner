using FluentResults;
using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public interface INetworkScannerFactory
    {
        IResult<NetworkScanner> CreateBasedOnIpRange(IpRange range);
    }
}
