using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Args;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace IpScanner.Ui.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private int _progress;
        private string _ipRange;
        private string _searchText;
        private readonly IIpScannerFactory _ipScannerFactory;
        private ObservableCollection<ScannedDevice> _scannedDevices;
        private ObservableCollection<ScannedDevice> _temporaryCollection;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MainPageViewModel(IIpScannerFactory factory)
        {
            Progress = 0;
            IpRange = string.Empty;
            SearchText = string.Empty;
            ScanCommand = new AsyncRelayCommand(ScanAsync);
            CancelCommand = new RelayCommand(Cancel);
            ScannedDevices = new ObservableCollection<ScannedDevice>();
            _temporaryCollection = new ObservableCollection<ScannedDevice>();

            _ipScannerFactory = factory;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public string IpRange
        {
            get => _ipRange;
            set => SetProperty(ref _ipRange, value);
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

        public ObservableCollection<ScannedDevice> ScannedDevices
        {
            get => _scannedDevices;
            set => SetProperty(ref _scannedDevices, value);
        }

        public AsyncRelayCommand ScanCommand { get; }

        public RelayCommand CancelCommand { get; }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        private async Task ScanAsync()
        {
            var scanner = _ipScannerFactory.CreateBasedOnIpRange(IpRange);
            scanner.DeviceScanned += DeviceScannedHandler;

            await scanner.StartAsync(_cancellationTokenSource.Token);
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private void DeviceScannedHandler(object sender, ScannedDeviceEventArgs e)
        {
            DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
            {
                ScannedDevices.Add(e.ScannedDevice);
                Progress = e.CurrentProgress;
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
    }
}
