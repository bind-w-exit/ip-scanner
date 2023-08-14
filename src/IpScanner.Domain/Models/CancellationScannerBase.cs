using System.Net;
using System.Threading.Tasks;
using System.Threading;
using IpScanner.Domain.Enums;
using System.Net.NetworkInformation;
namespace IpScanner.Domain.Models
{
    public abstract class CancellationScannerBase
    {
        private TaskCompletionSource<bool> _pauseTcs = new TaskCompletionSource<bool>();

        public CancellationScannerBase()
        {
            _pauseTcs.SetResult(true);
        }

        public virtual void Pause()
        {
            _pauseTcs = new TaskCompletionSource<bool>();
        }

        public virtual void Resume()
        {
            _pauseTcs.SetResult(true);
        }

        protected async Task<ScannedDevice> ScanSpecificIpAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            PhysicalAddress macAddress = await GetMacAddress(destination, cancellationToken);
            await WaitOrCancelIfRequested(cancellationToken);

            if (macAddress == PhysicalAddress.None)
            {
                return new ScannedDevice(destination);
            }

            string manufacturer = await GetManufacturer(macAddress);
            await WaitOrCancelIfRequested(cancellationToken);

            string name = await GetHostname(destination);
            await WaitOrCancelIfRequested(cancellationToken);

            return new ScannedDevice(DeviceStatus.Online, name, destination, manufacturer, macAddress, string.Empty);
        }

        protected abstract Task<PhysicalAddress> GetMacAddress(IPAddress destination, CancellationToken cancellation);

        protected abstract Task<string> GetManufacturer(PhysicalAddress macAddress);

        protected abstract Task<string> GetHostname(IPAddress destination);

        private async Task WaitOrCancelIfRequested(CancellationToken cancellationToken)
        {
            await _pauseTcs.Task;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
