using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Args;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using System;
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
        private bool _showDetails;
        private readonly INetworkScannerFactory _ipScannerFactory;
        private FilteredCollection<ScannedDevice> _filteredDevices;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ScanPageViewModel(INetworkScannerFactory factory, IMessenger messenger)
        {
            Progress = 0;
            ShowDetails = false;
            IpRange = string.Empty;
            SearchText = string.Empty;
            ScannedDevices = new FilteredCollection<ScannedDevice>();

            _ipScannerFactory = factory;
            _cancellationTokenSource = new CancellationTokenSource();

            RegisterMessages(messenger);
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
            set => SetProperty(ref _searchText, value);
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

        public bool ShowDetails
        {
            get => _showDetails;
            set => SetProperty(ref _showDetails, value);
        }

        public FilteredCollection<ScannedDevice> ScannedDevices
        {
            get => _filteredDevices;
            set => SetProperty(ref _filteredDevices, value);
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
                ScannedDevices.AddItem(e.ScannedDevice);
                Progress = ScannedDevices.Count;
            });
        }

        private void EnableSubnetMask()
        {
            IpRange = "192.168.0.1-254, 26.0.0.1-26.255.255.254";
        }

        private void EnableSubnetClassCMask()
        {
            IpRange = "192.168.0.1-254, 26.0.0.1-254";
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<FilterMessage>(this, OnFilterMessage);
            messenger.Register<DetailsPageVisibilityMessage>(this, OnDetailsPageVisibilityMessage);
        }

        private void OnFilterMessage(object sender, FilterMessage message)
        {
            if(message.FilterStatus)
            {
                ScannedDevices.AddFilter(message.Filter);
            }
            else
            {
                ScannedDevices.RemoveFilter(message.Filter);
            }

            ScannedDevices.RefreshFilteredItems();
        }

        private void OnDetailsPageVisibilityMessage(object sender, DetailsPageVisibilityMessage message)
        {
            ShowDetails = message.Visible;
        }
    }
}
