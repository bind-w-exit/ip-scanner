using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using IpScanner.Domain.Enums;
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

        private TaskCompletionSource<bool> _pauseTcs = new TaskCompletionSource<bool>();

        public NetworkScanner(IEnumerable<IPAddress> toScan, IManufactorRepository manufactorReceiver,
            IMacAddressRepository macAddressScanner, IHostRepository hostRepository)
        {
            _ipsToScan = toScan;
            _manufactorRepository = manufactorReceiver;
            _macAddressRepository = macAddressScanner;
            _hostRepository = hostRepository;

            _pauseTcs.SetResult(true);
        }

        public IReadOnlyCollection<IPAddress> ScannedIps { get => _ipsToScan.ToList(); }

        public event EventHandler<ScannedDeviceEventArgs> DeviceScanned;
        public event EventHandler ScanningFinished;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Task> tasks = _ipsToScan.Select(async destination => await ScanAndHandleCancellationAsync(destination, cancellationToken));
            await Task.WhenAll(tasks);

            ScanningFinished?.Invoke(this, EventArgs.Empty);
        }

        public void Pause()
        {
            _pauseTcs = new TaskCompletionSource<bool>();
        }

        public void Resume()
        {
            _pauseTcs.SetResult(true);
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

        private async Task<ScannedDevice> ScanSpecificIpAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            PhysicalAddress macAddress = await _macAddressRepository.GetMacAddressAsync(destination, cancellationToken);
            await _pauseTcs.Task;
            cancellationToken.ThrowIfCancellationRequested();
            if (macAddress == PhysicalAddress.None)
            {
                return new ScannedDevice(destination);
            }

            cancellationToken.ThrowIfCancellationRequested();
            string manufacturer = await _manufactorRepository.GetManufacturerOrEmptyStringAsync(macAddress);
            await _pauseTcs.Task;

            cancellationToken.ThrowIfCancellationRequested();
            string name = await GetHostname(destination);
            await _pauseTcs.Task;
            cancellationToken.ThrowIfCancellationRequested();

            return new ScannedDevice(DeviceStatus.Online, name, destination, manufacturer, macAddress, string.Empty);
        }

        private async Task<string> GetHostname(IPAddress destination)
        {
            IPHostEntry hostEntry = await _hostRepository.GetHostAsync(destination);
            return hostEntry.HostName;
        }
    }
}
