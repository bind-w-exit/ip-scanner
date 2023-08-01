using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Args;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace IpScanner.Ui.ViewModels
{
    public class ScanPageViewModel : ValidationViewModel, IDisposable
    {
        private int _progress;
        private string _ipRange;
        private string _searchText;
        private int _countOfScannedIps;
        private readonly INetworkScannerFactory _ipScannerFactory;
        private ObservableCollection<ScannedDevice> _scannedDevices;
        private ObservableCollection<ScannedDevice> _temporaryCollection;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ScanPageViewModel(INetworkScannerFactory factory)
        {
            Progress = 0;
            CountOfScannedIps = 100;
            IpRange = string.Empty;
            SearchText = string.Empty;
            ScannedDevices = new ObservableCollection<ScannedDevice>();
            _temporaryCollection = new ObservableCollection<ScannedDevice>();

            _ipScannerFactory = factory;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public string IpRange
        {
            get => _ipRange;
            set
            {
                HasValidationError = false;
                SetProperty(ref _ipRange, value);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterDevices();
            }
        }

        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public int CountOfScannedIps
        {
            get => _countOfScannedIps;
            set => SetProperty(ref _countOfScannedIps, value);
        }

        public ObservableCollection<ScannedDevice> ScannedDevices
        {
            get => _scannedDevices;
            set => SetProperty(ref _scannedDevices, value);
        }

        public AsyncRelayCommand ScanCommand { get => new AsyncRelayCommand(ScanAsync); }

        public RelayCommand CancelCommand { get => new RelayCommand(CancelScanning); }

        public RelayCommand SetSubnetMask { get => new RelayCommand(EnableSubnetMask); }

        public RelayCommand SetSubnetClassCMask { get => new RelayCommand(EnableSubnetClassCMask); }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        private async Task ScanAsync()
        {
            try
            {
                var scanner = _ipScannerFactory.CreateBasedOnIpRange(new IpRange(IpRange));
                scanner.DeviceScanned += DeviceScannedHandler;
                CountOfScannedIps = scanner.ScannedIps.Count;

                await scanner.StartAsync(_cancellationTokenSource.Token);
            }
            catch (IpValidationException)
            {
                HasValidationError = true;
            }
        }

        private void CancelScanning() => _cancellationTokenSource.Cancel();

        private void DeviceScannedHandler(object sender, ScannedDeviceEventArgs e)
        {
            DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
            {
                ScannedDevices.Add(e.ScannedDevice);
                Progress = ScannedDevices.Count;
            });
        }

        private void FilterDevices()
        {
            if (ScannedDevices == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(SearchText))
            {
                ScannedDevices.Clear();
                AddDivicesToView(_temporaryCollection);
            }
            else
            {
                var filteredDevices = _scannedDevices.Where(_scannedDevices => _scannedDevices.Name.Contains(SearchText)).ToList();

                _temporaryCollection = new ObservableCollection<ScannedDevice>(_scannedDevices);

                ScannedDevices.Clear();
                AddDivicesToView(filteredDevices);
            }
        }

        private void AddDivicesToView(IEnumerable<ScannedDevice> scannedDevices)
        {
            foreach (var item in scannedDevices)
            {
                ScannedDevices.Add(item);
            }
        }

        private void EnableSubnetMask()
        {
            IpRange = "192.168.0.1-254, 26.0.0.1-26.255.255.254";
        }

        private void EnableSubnetClassCMask()
        {
            IpRange = "192.168.0.1-254, 26.0.0.1-254";
        }
    }
}
