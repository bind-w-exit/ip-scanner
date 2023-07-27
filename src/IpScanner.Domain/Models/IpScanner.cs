using IpScanner.Domain.Enums;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Extensions;
using IpScanner.Domain.Interfaces;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain.Models
{
    public class IpScanner : IDisposable
    {
        private readonly int _startHostId;
        private readonly int _endHostId;
        private readonly ILazyResultProvider _resultProvider;
        private readonly IManufactorReceiver _manufactorReceiver;
        private readonly IMacAddressScanner _macAddressScanner;

        public IpScanner(int startHost, int endHost, ILazyResultProvider progressProvider,
            IManufactorReceiver manufactorReceiver, IMacAddressScanner macAddressScanner)
        {
            _startHostId = startHost;
            _endHostId = endHost;

            _resultProvider = progressProvider;
            _manufactorReceiver = manufactorReceiver;
            _macAddressScanner = macAddressScanner;
        }

        public static IpScanner Create(IPAddress start, IPAddress end, ILazyResultProvider progressProvider, 
            IManufactorReceiver manufactorReceiver, IMacAddressScanner macAddressScanner)
        {
            int startHostId = start.GetHostId();
            int endHostId = end.GetHostId();

            return new IpScanner(startHostId, endHostId, progressProvider, manufactorReceiver, macAddressScanner);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (int hostId = _startHostId; hostId <= _endHostId; hostId++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                IPAddress destination = hostId.GetLocalIPAddress();
                ScannedDevice scannedDevice = await ScanDeviceAsync(destination);

                _resultProvider.Report(scannedDevice, hostId);
            }
        }

        private async Task<ScannedDevice> ScanDeviceAsync(IPAddress destination)
        {
            try
            {
                PhysicalAddress macAddress = await _macAddressScanner.GetMacAddressAsync(destination);
                string manufacturer = await _manufactorReceiver.GetManufacturerOrEmptyStringAsync(macAddress);

                return new ScannedDevice(DeviceStatus.Online, destination.ToString(), destination, 
                    manufacturer, macAddress, string.Empty);
            }
            catch (MacAddressNotFoundException)
            {
                return new ScannedDevice(destination);
            }
        }

        public void Dispose()
        {
            _manufactorReceiver.Dispose();
        }
    }
}
