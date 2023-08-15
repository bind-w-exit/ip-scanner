using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Models;
using System.Threading;
using IpScanner.Domain.Args;
using System;
using IpScanner.Ui.ObjectModels;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Core;
using IpScanner.Domain.Enums;
using System.Windows.Input;
using System.Net;
using IpScanner.Domain.Validators;
using Windows.ApplicationModel.Core;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Ui.Messages;

namespace IpScanner.Ui.ViewModels.Modules.Scanning
{
    public class ScanningModule : ObservableObject
    {
        private bool _paused;
        private bool _stopping;
        private bool _currentlyScanning;
        private ScannedDevice _selectedDevice;
        private readonly INetworkScanner _networkScanner;
        private readonly IValidator<IpRange> _ipRangeValidator;
        private readonly ProgressModule _progressModule;
        private readonly IpRangeModule _ipRangeModule;
        private readonly FavoritesDevicesModule _favoritesDevicesModule;
        private FilteredCollection<ScannedDevice> _scannedDevices;
        private CancellationTokenSource _cancellationTokenSource;

        public ScanningModule(IMessenger messanger, INetworkScanner networkScanner, IValidator<IpRange> ipRangeValidator, 
            ProgressModule progressModule, IpRangeModule ipRangeModule, FavoritesDevicesModule favoritesDevicesModule)
        {
            _networkScanner = networkScanner;
            _ipRangeValidator = ipRangeValidator;
            _progressModule = progressModule;
            _ipRangeModule = ipRangeModule;
            Paused = false;
            CurrentlyScanning = false;
            Stopping = false;

            _networkScanner.DeviceScanned += DeviceScannedHandler;
            _networkScanner.ScanningFinished += ScanningFinished;
            _cancellationTokenSource = new CancellationTokenSource();

            messanger.Register<DeviceSelectedMessage>(this, (sender, message) =>
            {
                _selectedDevice = message.Device;
            });
            _favoritesDevicesModule = favoritesDevicesModule;
        }

        public bool CurrentlyScanning
        {
            get => _currentlyScanning;
            set => SetProperty(ref _currentlyScanning, value);
        }

        public bool Paused
        {
            get => _paused;
            set => SetProperty(ref _paused, value);
        }

        public bool Stopping
        {
            get => _stopping;
            set => SetProperty(ref _stopping, value);
        }

        public ICommand ScanCommand => new RelayCommand(Scan);

        public ICommand RescanCommand => new AsyncRelayCommand(RescanAsync);

        public ICommand CancelCommand => new RelayCommand(Cancel);

        public ICommand PauseCommand => new RelayCommand(Pause);

        public ICommand ResumeCommand => new RelayCommand(Resume);

        public List<ScannedDevice> Devices => _scannedDevices.ToList();

        public void InitializeCollection(FilteredCollection<ScannedDevice> scannedDevices)
        {
            _scannedDevices = scannedDevices;
        }

        private void Scan()
        {
            InitiateScanning();
            var collectionToModify = GetSelectedCollection();

            List<IPAddress> addresses = _favoritesDevicesModule.DisplayFavorites 
                ? collectionToModify.Select(f => f.Ip).ToList()
                : GetAddressesBasedOnIpRange().ToList();

            collectionToModify.Clear();

            if (addresses == null)
            {
                OnValidationError();
            }
            else
            {
                _progressModule.SetTotalCountOfIps(addresses.Count());
                StartScanning(addresses);
            }
        }

        private async Task RescanAsync()
        {
            InitiateScanning();

            IEnumerable<IPAddress> addresses = new List<IPAddress> { _selectedDevice.Ip };
            _progressModule.SetTotalCountOfIps(addresses.Count());

            await _networkScanner.StartAsync(addresses, _cancellationTokenSource.Token);
        }

        private void InitiateScanning()
        {
            CurrentlyScanning = true;
            _progressModule.ResetProgress();
        }

        private void StartScanning(IEnumerable<IPAddress> addresses)
        {
            var thread = new Thread(async () =>
            {
                await _networkScanner.StartAsync(addresses, _cancellationTokenSource.Token);
            });

            thread.Start();
        }

        private void Cancel()
        {
            Stopping = true;
            _cancellationTokenSource.Cancel();
            ResetCancellationTokenSource();
        }

        private void Pause()
        {
            Paused = true;
            _networkScanner.Pause();
        }

        private void Resume()
        {
            Paused = false;
            _networkScanner.Resume();
        }

        private void ResetCancellationTokenSource()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnValidationError()
        {
            _ipRangeModule.ValidationModule.HasValidationError = true;
            CurrentlyScanning = false;
        }

        private void AddDevice(ScannedDevice scannedDevice)
        {
            FilteredCollection<ScannedDevice> currentCollection = GetSelectedCollection();

            ScannedDevice exists = currentCollection.FirstOrDefault(device => device.Ip.Equals(scannedDevice.Ip));
            if (exists != null)
            {
                currentCollection.ReplaceItem(exists, scannedDevice);
            }
            else
            {
                if (scannedDevice.Status == DeviceStatus.Online)
                {
                    currentCollection.Insert(0, scannedDevice);
                }
                else
                {
                    currentCollection.Add(scannedDevice);
                }
            }

            _progressModule.IncreaseProgress(scannedDevice.Status);
        }

        private IEnumerable<IPAddress> GetAddressesBasedOnIpRange()
        {
            var ipRange = new IpRange(_ipRangeModule.IpRange);
            bool validationResult = _ipRangeValidator.Validate(ipRange);
            if (validationResult == false)
            {
                return null;
            }

            return ipRange.GenerateIPAddresses();
        }

        private void FinishScanning()
        {
            CurrentlyScanning = false;
            Paused = false;
            Stopping = false;
        }

        private FilteredCollection<ScannedDevice> GetSelectedCollection()
        {
            return _favoritesDevicesModule.DisplayFavorites
                ? _favoritesDevicesModule.FavoritesDevices
                : _scannedDevices;
        }

        private async void ScanningFinished(object sender, EventArgs e)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, FinishScanning);
        }

        private async void DeviceScannedHandler(object sender, ScannedDeviceEventArgs e)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => AddDevice(e.ScannedDevice));
        }
    }
}
