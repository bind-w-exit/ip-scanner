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
    public class NetworkScanner
    {
        private readonly IEnumerable<IPAddress> _ipsToScan;
        private readonly IManufactorRepository _manufactorRepository;
        private readonly IMacAddressRepository _macAddressRepository;
        private readonly IHostRepository _hostRepository;

        public NetworkScanner(IEnumerable<IPAddress> toScan, IManufactorRepository manufactorReceiver,
            IMacAddressRepository macAddressScanner, IHostRepository hostRepository)
        {
            _ipsToScan = toScan;
            _manufactorRepository = manufactorReceiver;
            _macAddressRepository = macAddressScanner;
            _hostRepository = hostRepository;
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
            PhysicalAddress macAddress = await GetMacAddressAsync(destination);
            if (macAddress == PhysicalAddress.None)
            {
                return new ScannedDevice(destination);
            }

            string manufacturer = await _manufactorRepository.GetManufacturerOrEmptyStringAsync(macAddress);
            string name = await GetHostnameOrIpAddress(destination);

            return new ScannedDevice(DeviceStatus.Online, name, destination, manufacturer, macAddress, string.Empty);
        }

        private async Task<string> GetHostnameOrIpAddress(IPAddress destination)
        {
            try
            {
                IPHostEntry hostEntry = await _hostRepository.GetHostAsync(destination);
                return hostEntry.HostName;
            }
            catch (HostNotFoundException)
            {
                return destination.ToString();
            }
        }

        private async Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination)
        {
            try
            {
                return await _macAddressRepository.GetMacAddressAsync(destination);
            }
            catch (MacAddressNotFoundException)
            {
                return PhysicalAddress.None;
            }
        }
    }
}
