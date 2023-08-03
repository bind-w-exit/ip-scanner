using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Args;
using IpScanner.Domain.Enums;
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
        private bool _currentlyScanning;
        private int _countOfUnknownDevices;
        private int _countOfOnlineDevices;
        private int _countOfOfflineDevices;
        private ScannedDevice _selectedDevice;
        private readonly INetworkScannerFactory _ipScannerFactory;
        private FilteredCollection<ScannedDevice> _filteredDevices;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IMessenger _messanger;
        private readonly ItemFilter<ScannedDevice> _searchFilter;

        public ScanPageViewModel(INetworkScannerFactory factory, IMessenger messenger)
        {
            _messanger = messenger;

            CountOfScannedIps = 0;
            TotalCountOfIps = int.MaxValue;
            ShowDetails = false;
            CurrentlyScanning = false;
            IpRange = string.Empty;
            SearchText = string.Empty;
            SelectedDevice = new ScannedDevice(System.Net.IPAddress.Any);
            ScannedDevices = new FilteredCollection<ScannedDevice>();

            _ipScannerFactory = factory;
            _cancellationTokenSource = new CancellationTokenSource();

            _searchFilter = new ItemFilter<ScannedDevice>(device => device.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
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
            set
            {
                bool isValueSet = SetProperty(ref _searchText, value);
                if (!isValueSet || ScannedDevices == null)
                    return;

                UpdateScannedDevicesSearchFilter();
            }
        }

        public int CountOfScannedIps
        {
            get => _progress;
            set
            {
                if(SetProperty(ref _progress, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }
            }
        }

        public int TotalCountOfIps
        {
            get => _countOfScannedIps;
            set => SetProperty(ref _countOfScannedIps, value);
        }

        public bool ShowDetails
        {
            get => _showDetails;
            set => SetProperty(ref _showDetails, value);
        }

        public bool CurrentlyScanning
        {
            get => _currentlyScanning;
            set => SetProperty(ref _currentlyScanning, value);
        }

        public int CountOfUnknownDevices
        {
            get => _countOfUnknownDevices;
            set
            {
                if(SetProperty(ref _countOfUnknownDevices, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }   
            }
        }

        public int CountOfOnlineDevices
        {
            get => _countOfOnlineDevices;
            set => SetProperty(ref _countOfOnlineDevices, value);
        }

        public int CountOfOfflineDevices
        {
            get => _countOfOfflineDevices;
            set
            {
                if(SetProperty(ref _countOfOfflineDevices, value))
                {
                    OnPropertyChanged(nameof(ProgressString));
                }
            }
        }

        public string ProgressString
        {
            get => $"{CalculateProgress()}%, {CountOfOfflineDevices} dead, {CountOfUnknownDevices} unknown";
        }

        public ScannedDevice SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _messanger.Send(new DeviceSelectedMessage(value));
                SetProperty(ref _selectedDevice, value);
            }
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

        private async Task TryScanningAsync()
        {
            InitiateScanning();

            NetworkScanner scanner = CreateScanner();
            TotalCountOfIps = scanner.ScannedIps.Count;

            await scanner.StartAsync(_cancellationTokenSource.Token);

            FinishScanning();
        }

        private void InitiateScanning()
        {
            ResetProgress();
            ScannedDevices.Clear();
            CurrentlyScanning = true;
        }

        private NetworkScanner CreateScanner()
        {
            NetworkScanner scanner = _ipScannerFactory.CreateBasedOnIpRange(new IpRange(IpRange));
            scanner.DeviceScanned += DeviceScannedHandler;

            return scanner;
        }

        private void FinishScanning()
        {
            CurrentlyScanning = false;
        }

        private void OnValidationError()
        {
            HasValidationError = true;
            CurrentlyScanning = false;
        }

        private void ResetProgress()
        {
            CountOfScannedIps = 0;
            CountOfUnknownDevices = 0;
            CountOfOnlineDevices = 0;
            CountOfOfflineDevices = 0;
            TotalCountOfIps = int.MaxValue;
        }

        private void CancelScanning()
        {
            CancelCurrentTask();
            ResetCancellationTokenSource();
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
                ScannedDevices.AddItem(e.ScannedDevice);
                CountOfScannedIps = ScannedDevices.Count;
                IncreaseCountOfSpecificDevices(e.ScannedDevice.Status);
            });
        }

        private void IncreaseCountOfSpecificDevices(DeviceStatus status)
        {
            switch(status)
            {
                case DeviceStatus.Unknown:
                    CountOfUnknownDevices++;
                    break;
                case DeviceStatus.Online:
                    CountOfOnlineDevices++;
                    break;
                case DeviceStatus.Offline:
                    CountOfOfflineDevices++;
                    break;
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

        private double CalculateProgress()
        {
            if(TotalCountOfIps == 0)
            {
                return 0;
            }

            return Math.Round(((double)CountOfScannedIps / TotalCountOfIps) * 100, 2);
        }

        private void UpdateScannedDevicesSearchFilter()
        {
            ScannedDevices.RemoveFilter(_searchFilter);
            ScannedDevices.AddFilter(_searchFilter);
            ScannedDevices.RefreshFilteredItems();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }
    }
}
