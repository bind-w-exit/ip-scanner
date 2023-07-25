using IpScanner.Domain.Interfaces;
using System.Diagnostics;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.WiFiDirect;

namespace IpScanner.Domain.Models
{
    public class IpScanner
    {
        private readonly int _maxHostId;
        private readonly ILazyResultProvider _resultProvider;

        public IpScanner(int maxHostId, ILazyResultProvider progressProvider)
        {
            _maxHostId = maxHostId;
            _resultProvider = progressProvider;
        }

        public static IpScanner Create(string ip, ILazyResultProvider progressProvider)
        {
            var maxHostId = int.Parse(ip.Split('.').Last());
            return new IpScanner(maxHostId, progressProvider);
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            var deviceSelector = Windows.Devices.WiFiDirect.WiFiDirectDevice.GetDeviceSelector(WiFiDirectDeviceSelectorType.AssociationEndpoint);

            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(deviceSelector);

            foreach (var device in devices)
            {
                var name = device.Name;

                var scannedDevice = new ScannedDevice("Online", name, IPAddress.Any, string.Empty, string.Empty, string.Empty);

                _resultProvider.Report(scannedDevice, 10);
            }

        }
    }
}
