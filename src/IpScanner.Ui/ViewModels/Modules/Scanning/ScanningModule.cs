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

namespace IpScanner.Ui.ViewModels.Modules
{
    public class ScanningModule : ObservableObject
    {
        private bool _currentlyScanning;
        private bool _paused;
        private bool _stopping;
        private readonly INetworkScanner _networkScanner;
        private readonly IValidator<IpRange> _ipRangeValidator;
        private readonly ProgressModule _progressModule;
        private readonly IpRangeModule _ipRangeModule;
        private FilteredCollection<ScannedDevice> _scannedDevices;
        private CancellationTokenSource _cancellationTokenSource;

        public ScanningModule(INetworkScanner networkScanner, IValidator<IpRange> ipRangeValidator, ProgressModule progressModule, IpRangeModule ipRangeModule)
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

            var ipRange = new IpRange(_ipRangeModule.IpRange);
            bool validationResult = _ipRangeValidator.Validate(ipRange);  
            if (validationResult == false)
            {
                OnValidationError();
                return;
            }

            IEnumerable<IPAddress> addresses = ipRange.GenerateIPAddresses();
            _progressModule.TotalCountOfIps = addresses.Count();

            StartScanning(addresses);
        }

        private void InitiateScanning()
        {
            CurrentlyScanning = true;
            _progressModule.ResetProgress();
            _scannedDevices.Clear();
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
            if (scannedDevice.Status == DeviceStatus.Online)
            {
                _scannedDevices.Insert(0, scannedDevice);
            }
            else
            {
                _scannedDevices.Add(scannedDevice);
            }

            _progressModule.UpdateProgress(_scannedDevices.Count, scannedDevice.Status);
        }

        private void FinishScanning()
        {
            CurrentlyScanning = false;
            Paused = false;
            Stopping = false;
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
