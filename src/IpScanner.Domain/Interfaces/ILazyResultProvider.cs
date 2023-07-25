using IpScanner.Domain.Models;

namespace IpScanner.Domain.Interfaces
{
    public interface ILazyResultProvider
    {
        void Report(ScannedDevice device, int progress);
    }
}
