using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using FluentResults;
using Windows.UI.Core;
using IpScanner.Domain.Enums;

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
        private NetworkScanner _scanner;

        public ScanningModule(ProgressModule progressModule, IpRangeModule ipRangeModule, INetworkScannerFactory factory)
        {
            _progressModule = progressModule;
            _ipRangeModule = ipRangeModule;

            _ipScannerFactory = factory;

            CurrentlyScanning = false;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool CurrentlyScanning
        {
            get => _currentlyScanning;
            set => SetProperty(ref _currentlyScanning, value);
        }

        public List<ScannedDevice> Devices => _scannedDevices.ToList();

        public RelayCommand ScanCommand => new RelayCommand(Scan);

        public RelayCommand CancelCommand => new RelayCommand(CancelScanning);

        public RelayCommand PauseCommand => new RelayCommand(Pause);

        public void Dispose() => _cancellationTokenSource.Dispose();

        public void InitializeCollection(FilteredCollection<ScannedDevice> scannedDevices)
        {
            _scannedDevices = scannedDevices;
        }

        private void Scan()
        {
            InitiateScanning();

            _scanner = CreateScannerIfErrorReturnNull();
            if (_scanner == null)
            {
                OnValidationError();
                return;
            }

            _progressModule.TotalCountOfIps = _scanner.ScannedIps.Count;

            var thread = new Thread(async () =>
            {
                await _scanner.StartAsync(_cancellationTokenSource.Token);
            });

            thread.Start();
        }

        private void CancelScanning()
        {
            _cancellationTokenSource.Cancel();
            ResetCancellationTokenSource();
        }

        private int counter = 0;
        private void Pause()
        {
            if(counter % 2 == 0)
                _scanner.Pause();
            else
                _scanner.Resume();

            counter++;
        }

        private void InitiateScanning()
        {
            _progressModule.ResetProgress();
            _scannedDevices.Clear();
            CurrentlyScanning = true;
        }

        private NetworkScanner CreateScannerIfErrorReturnNull()
        {
            IResult<NetworkScanner> result = _ipScannerFactory.CreateBasedOnIpRange(new IpRange(_ipRangeModule.IpRange));
            if (result.IsFailed)
            {
                return null;
            }

            NetworkScanner scanner = result.Value;
            scanner.DeviceScanned += DeviceScannedHandler;
            scanner.ScanningFinished += ScanningFinished;

            return scanner;
        }

        private async void ScanningFinished(object sender, EventArgs e)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CurrentlyScanning = false;
            });
        }

        private void ResetCancellationTokenSource()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async void DeviceScannedHandler(object sender, ScannedDeviceEventArgs e)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if(e.ScannedDevice.Status == DeviceStatus.Online)
                {
                    _scannedDevices.Insert(0, e.ScannedDevice);
                }
                else
                {
                    _scannedDevices.Add(e.ScannedDevice);
                }

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
