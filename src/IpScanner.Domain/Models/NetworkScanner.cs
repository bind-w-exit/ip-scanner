using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using IpScanner.Domain.Interfaces;
using System.Linq;
using IpScanner.Domain.Args;

namespace IpScanner.Domain.Models
{
    public class NetworkScanner : CancellationScannerBase, INetworkScanner
    {
        private readonly IHostRepository _hostRepository;
        private readonly IManufactorRepository _manufactorRepository;
        private readonly IMacAddressRepository _macAddressRepository;

        public NetworkScanner(IManufactorRepository manufactorReceiver, IMacAddressRepository macAddressScanner, IHostRepository hostRepository)
            : base()
        {
            _manufactorRepository = manufactorReceiver;
            _macAddressRepository = macAddressScanner;
            _hostRepository = hostRepository;
        }


        public event EventHandler ScanningFinished;

        public event EventHandler<ScannedDeviceEventArgs> DeviceScanned;

        public async Task StartAsync(IEnumerable<IPAddress> addresses, CancellationToken cancellationToken)
        {
            IEnumerable<Task> tasks = addresses.Select(async destination => await ScanAndHandleCancellationAsync(destination, cancellationToken));
            await Task.WhenAll(tasks);

            ScanningFinished?.Invoke(this, EventArgs.Empty);
        }

        private async Task ScanAndHandleCancellationAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            try
            {
                ScannedDevice scannedDevice = await ScanSpecificIpAsync(destination, cancellationToken);
                DeviceScanned?.Invoke(this, new ScannedDeviceEventArgs(scannedDevice));
            }
            catch (OperationCanceledException)
            { }
        }

        protected override async Task<PhysicalAddress> GetMacAddress(IPAddress destination, CancellationToken cancellation)
        {
            return await _macAddressRepository.GetMacAddressAsync(destination, cancellation);
        }

        protected override async Task<string> GetManufacturer(PhysicalAddress macAddress)
        {
            return await _manufactorRepository.GetManufacturerOrEmptyStringAsync(macAddress);
        }

        protected override async Task<string> GetHostname(IPAddress destination)
        {
            IPHostEntry hostEntry = await _hostRepository.GetHostAsync(destination);
            return hostEntry.HostName;
        }
    }
}
