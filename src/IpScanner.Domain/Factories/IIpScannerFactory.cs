namespace IpScanner.Domain.Factories
{
    public interface IIpScannerFactory
    {
        Models.IpScanner CreateBasedOnIpRange(string ipRange);
    }
}
