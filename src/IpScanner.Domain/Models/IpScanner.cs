using IpScanner.Domain.Enums;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Extensions;
using IpScanner.Domain.Interfaces;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain.Models
{
    public class IpScanner
    {
        private readonly int _startHostId;
        private readonly int _endHostId;
        private readonly ILazyResultProvider _resultProvider;
        private readonly IManufactorReceiver _manufactorReceiver;
        private readonly IMacAddressScanner _macAddressScanner;

        public IpScanner(int startHost, int endHost, ILazyResultProvider progressProvider)
        {
            _startHostId = startHost;
            _endHostId = endHost;

            _resultProvider = progressProvider;
        }

        public static IpScanner Create(IPAddress start, IPAddress end, ILazyResultProvider progressProvider)
        {
            var startHostId = int.Parse(start.ToString().Split('.').Last());
            var endHostId = int.Parse(end.ToString().Split('.').Last());

            return new IpScanner(startHostId, endHostId, progressProvider);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => StartScanning(cancellationToken));
        }

        private void StartScanning(CancellationToken cancellationToken)
        {
            for (int hostId = _startHostId; hostId <= _endHostId; hostId++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                IPAddress destination = hostId.GetLocalIPAddress();
                ScannedDevice scannedDevice = ScanDevice(destination);

                _resultProvider.Report(scannedDevice, hostId);
            }
        }

        private ScannedDevice ScanDevice(IPAddress destination)
        {
            try
            {
                PhysicalAddress macAddress = _macAddressScanner.GetMacAddress(destination);
                string manufacturer = _manufactorReceiver.GetManufacturerOrEmptyString(macAddress);

                return new ScannedDevice(DeviceStatus.Online, destination.ToString(), destination, 
                    manufacturer, macAddress, string.Empty);
            }
            catch (MacAddressNotFoundException)
            {
                return new ScannedDevice(destination);
            }
        }
    }
}
