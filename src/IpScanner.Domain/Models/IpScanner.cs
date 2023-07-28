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
            int progress = 0;

            foreach (var destination in _ipsToScan)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                ScannedDevice scannedDevice = await ScanSpecificIpAsync(destination);
                DeviceScanned?.Invoke(this, new ScannedDeviceEventArgs(scannedDevice, progress++));
            }
        }

        private async Task<ScannedDevice> ScanSpecificIpAsync(IPAddress destination)
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
    }
}
