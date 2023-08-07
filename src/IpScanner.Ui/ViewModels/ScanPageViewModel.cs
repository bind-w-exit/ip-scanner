using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.ViewModels.Modules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Ui.ViewModels
{
    public class ScanPageViewModel : ObservableObject
    {
        private bool _showDetails;
        private bool _showMiscellaneous;
        private bool _showActions;
        private ScannedDevice _selectedDevice;
        private FilteredCollection<ScannedDevice> _filteredDevices;
        private IpRangeModule _ipRangeModule;
        private SearchModule _searchModule;
        private ProgressModule _progressModule;
        private ScanningModule _scanningModule;
        private FavoritesDevicesModule _favoritesDevicesModule;
        private readonly IMessenger _messanger;
        private readonly INetworkScannerFactory _networkScannerFactory;
        private readonly IFileService<ScannedDevice> _fileService;

        public ScanPageViewModel(IMessenger messenger, IFileService<ScannedDevice> fileService, 
            FavoritesDevicesModule favoritesDevicesModule,ProgressModule progressModule, 
            IpRangeModule ipRangeModule, ScanningModule scanningModule, INetworkScannerFactory networkScannerFactory)
        {
            _messanger = messenger;
            _fileService = fileService;
            _networkScannerFactory = networkScannerFactory;

            ShowDetails = false;
            ShowMiscellaneous = true;
            ShowActions = true;
            SelectedDevice = new ScannedDevice(System.Net.IPAddress.Any);
            ScannedDevices = new FilteredCollection<ScannedDevice>();

            IpRangeModule = ipRangeModule;
            ProgressModule = progressModule;
            FavoritesDevicesModule = favoritesDevicesModule;

            SearchModule = new SearchModule(ScannedDevices);

            ScanningModule = scanningModule;
            ScanningModule.InitializeCollection(ScannedDevices);

            RegisterMessages(messenger);
        }

        public bool ShowDetails
        {
            get => _showDetails;
            set => SetProperty(ref _showDetails, value);
        }

        public bool ShowMiscellaneous
        {
            get => _showMiscellaneous;
            set => SetProperty(ref _showMiscellaneous, value);
        }

        public bool ShowActions
        {
            get => _showActions;
            set => SetProperty(ref _showActions, value);
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

        public IpRangeModule IpRangeModule
        {
            get => _ipRangeModule;
            set => SetProperty(ref _ipRangeModule, value);
        }

        public SearchModule SearchModule
        {
            get => _searchModule;
            set => SetProperty(ref _searchModule, value);
        }

        public ProgressModule ProgressModule
        {
            get => _progressModule;
            set => SetProperty(ref _progressModule, value);
        }

        public ScanningModule ScanningModule
        {
            get => _scanningModule;
            set => SetProperty(ref _scanningModule, value);
        }

        public FavoritesDevicesModule FavoritesDevicesModule
        {
            get => _favoritesDevicesModule;
            set => SetProperty(ref _favoritesDevicesModule, value);
        }

        public FilteredCollection<ScannedDevice> ScannedDevices
        {
            get => _filteredDevices;
            set => SetProperty(ref _filteredDevices, value);
        }

        public AsyncRelayCommand SaveDeviceCommand => new AsyncRelayCommand(SaveDeviceAsync);

        private async Task SaveDeviceAsync()
        {
            await _fileService.SaveItemsAsync(new List<ScannedDevice> { SelectedDevice });
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<FilterMessage>(this, OnFilterMessage);
            messenger.Register<DetailsPageVisibilityMessage>(this, OnDetailsPageVisibilityMessage);
            messenger.Register<MiscellaneousBarVisibilityMessage>(this, OnMiscellaneousBarVisibilityMessage);
            messenger.Register<ActionsBarVisibilityMessage>(this, OnActionsBarVisibilityMessage);
            messenger.Register<DevicesLoadedMessage>(this, OnDevicesLoadedMessage);
            messenger.Register<ScanFromFileMessage>(this, OnScanFromFileMessage);
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

        private void OnMiscellaneousBarVisibilityMessage(object sender, MiscellaneousBarVisibilityMessage message)
        {
            ShowMiscellaneous = message.Visible;
        }

        private void OnActionsBarVisibilityMessage(object sender, ActionsBarVisibilityMessage message)
        {
            ShowActions = message.Visible;
        }

        private void OnDevicesLoadedMessage(object sender, DevicesLoadedMessage message)
        {
            ScannedDevices.Clear();
            ScannedDevices.AddRange(message.Devices);
        }

        private async void OnScanFromFileMessage(object sender, ScanFromFileMessage message)
        {
            IpRangeModule.IpRange = message.Content;
            await ScanningModule.ScanCommand.ExecuteAsync(this);
        }
    }
}
