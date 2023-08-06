using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Models;
using System.Threading.Tasks;
using System.Threading;
using IpScanner.Domain.Args;
using Windows.System;
using System;
using IpScanner.Domain.Factories;
using IpScanner.Ui.ObjectModels;
using System.Linq;
using System.Collections.Generic;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class ScanningModule : ObservableObject, IDisposable
    {
        private bool _currentlyScanning;
        private readonly INetworkScannerFactory _ipScannerFactory;
        private readonly ProgressModule _progressModule;
        private readonly IpRangeModule _ipRangeModule;
        private FilteredCollection<ScannedDevice> _scannedDevices;
        private CancellationTokenSource _cancellationTokenSource;

        public ScanningModule(ProgressModule progressModule, IpRangeModule ipRangeModule, INetworkScannerFactory factory)
        {
            _progressModule = progressModule;
            _ipRangeModule = ipRangeModule;

            _ipScannerFactory = factory;
            _cancellationTokenSource = new CancellationTokenSource();

            CurrentlyScanning = false;
        }

        public bool CurrentlyScanning
        {
            get => _currentlyScanning;
            set => SetProperty(ref _currentlyScanning, value);
        }

        public List<ScannedDevice> Devices
        {
            get => _scannedDevices.ToList();
        }

        public AsyncRelayCommand ScanCommand { get => new AsyncRelayCommand(ScanAsync); }

        public RelayCommand CancelCommand { get => new RelayCommand(CancelScanning); }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public void InitializeCollection(FilteredCollection<ScannedDevice> scannedDevices)
        {
            _scannedDevices = scannedDevices;
        }

        private async Task ScanAsync()
        {
            try
            {
                await TryScanningAsync();
            }
            catch (IpValidationException)
            {
                OnValidationError();
            }
        }

        private void CancelScanning()
        {
            CancelCurrentTask();
            ResetCancellationTokenSource();
        }

        private async Task TryScanningAsync()
        {
            InitiateScanning();

            NetworkScanner scanner = CreateScanner();
            _progressModule.TotalCountOfIps = scanner.ScannedIps.Count;

            await scanner.StartAsync(_cancellationTokenSource.Token);

            FinishScanning();
        }

        private void InitiateScanning()
        {
            _progressModule.ResetProgress();
            _scannedDevices.Clear();
            CurrentlyScanning = true;
        }

        private NetworkScanner CreateScanner()
        {
            NetworkScanner scanner = _ipScannerFactory.CreateBasedOnIpRange(new IpRange(_ipRangeModule.IpRange));
            scanner.DeviceScanned += DeviceScannedHandler;

            return scanner;
        }

        private void FinishScanning()
        {
            CurrentlyScanning = false;
        }

        private void CancelCurrentTask()
        {
            _cancellationTokenSource.Cancel();
            CurrentlyScanning = false;
        }

        private void ResetCancellationTokenSource()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void DeviceScannedHandler(object sender, ScannedDeviceEventArgs e)
        {
            DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
            {
                _scannedDevices.Add(e.ScannedDevice);
                _progressModule.UpdateProgress(_scannedDevices.Count, e.ScannedDevice.Status);
            });
        }

        private void OnValidationError()
        {
            _ipRangeModule.ValidationModule.HasValidationError = true;
            CurrentlyScanning = false;
        }
    }
}
