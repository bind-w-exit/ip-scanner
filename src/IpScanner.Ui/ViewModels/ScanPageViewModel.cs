using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.ViewModels.Modules;

namespace IpScanner.Ui.ViewModels
{
    public class ScanPageViewModel : ObservableObject, IDisposable
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

        public ScanPageViewModel(IMessenger messenger, INetworkScannerFactory factory, FavoritesDevicesModule favoritesDevicesModule, 
            ProgressModule progressModule, IpRangeModule ipRangeModule)
        {
            _messanger = messenger;

            ShowDetails = false;
            ShowMiscellaneous = true;
            ShowActions = true;
            SelectedDevice = new ScannedDevice(System.Net.IPAddress.Any);
            ScannedDevices = new FilteredCollection<ScannedDevice>();

            IpRangeModule = ipRangeModule;
            ProgressModule = progressModule;
            FavoritesDevicesModule = favoritesDevicesModule;

            SearchModule = new SearchModule(ScannedDevices);
            ScanningModule = new ScanningModule(ProgressModule, IpRangeModule, ScannedDevices, factory);

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

        public void Dispose() => ScanningModule.Dispose();

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<FilterMessage>(this, OnFilterMessage);
            messenger.Register<DetailsPageVisibilityMessage>(this, OnDetailsPageVisibilityMessage);
            messenger.Register<MiscellaneousBarVisibilityMessage>(this, OnMiscellaneousBarVisibilityMessage);
            messenger.Register<ActionsBarVisibilityMessage>(this, OnActionsBarVisibilityMessage);
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
    }
}
