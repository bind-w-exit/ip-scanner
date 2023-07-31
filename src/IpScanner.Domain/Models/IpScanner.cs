using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using IpScanner.Domain.Enums;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Interfaces;
using System.Linq;
using IpScanner.Domain.Args;

namespace IpScanner.Domain.Models
{
    public class IpScanner
    {
        private readonly IEnumerable<IPAddress> _ipsToScan;
        private readonly IManufactorReceiver _manufactorReceiver;
        private readonly IMacAddressScanner _macAddressScanner;

        public IpScanner(IEnumerable<IPAddress> toScan, IManufactorReceiver manufactorReceiver, IMacAddressScanner macAddressScanner)
        {
            _ipsToScan = toScan;

            _manufactorReceiver = manufactorReceiver;
            _macAddressScanner = macAddressScanner;
        }

        public IReadOnlyCollection<IPAddress> ScannedIps { get => _ipsToScan.ToList(); }

        public event EventHandler<ScannedDeviceEventArgs> DeviceScanned;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Task> tasks = _ipsToScan.Select(async destination =>
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                ScannedDevice scannedDevice = await ScanSpecificIpAsync(destination);
                DeviceScanned?.Invoke(this, new ScannedDeviceEventArgs(scannedDevice));
            });

            await Task.WhenAll(tasks);
        }

        private async Task<ScannedDevice> ScanSpecificIpAsync(IPAddress destination)
        {
            try
            {
                PhysicalAddress macAddress = await _macAddressScanner.GetMacAddressAsync(destination);
                string manufacturer = await _manufactorReceiver.GetManufacturerOrEmptyStringAsync(macAddress);
                string name = await GetHostnameOrIpAddress(destination);

                return new ScannedDevice(DeviceStatus.Online, name, destination, manufacturer, macAddress, string.Empty);
            }
            catch (MacAddressNotFoundException)
            {
                return new ScannedDevice(destination);
            }
        }

        private async Task<string> GetHostnameOrIpAddress(IPAddress destination)
        {
            try
            {
                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(destination);
                return hostEntry.HostName;
            }
            catch (Exception)
            {
                return destination.ToString();
            }
        }
    }
}
