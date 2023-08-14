using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Infrastructure.Settings;
using IpScanner.Ui.Extensions;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.Printing;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels.Modules.Scanning;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace IpScanner.Ui.ViewModels
{
    public class ScanPageViewModel : ObservableObject
    {
        private bool _showDetails;
        private bool _showMiscellaneous;
        private bool _showActions;
        private ScannedDevice _selectedDevice;
        private FilteredCollection<ScannedDevice> _filteredDevices;
        private readonly IpRangeModule _ipRangeModule;
        private readonly SearchModule _searchModule;
        private readonly ProgressModule _progressModule;
        private readonly ScanningModule _scanningModule;
        private readonly FavoritesDevicesModule _favoritesDevicesModule;
        private readonly IMessenger _messanger;
        private readonly IFileService _fileService;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;
        private readonly IPrintService<ScannedDevice> _printService;
        private readonly IUriOpenerService _uriOpenerService;
        private readonly IFtpService _ftpService;
        private readonly IDialogService _dialogService;
        private readonly ILocalizationService _localizationService;
        private readonly IRdpService _rdpService;
        private readonly IWakeOnLanService _wakeOnLanService;

        public ScanPageViewModel(IMessenger messenger, IFileService fileService, IPrintService<ScannedDevice> printService, IDeviceRepositoryFactory deviceRepositoryFactory, 
            IUriOpenerService uriOpenerService, IFtpService ftpService, IDialogService dialogService, ILocalizationService localizationService,
            IRdpService rdpService, IWakeOnLanService wakeOnLanService, FavoritesDevicesModule favoritesDevicesModule, ProgressModule progressModule,
            IpRangeModule ipRangeModule, ScanningModule scanningModule)
        {
            _messanger = messenger;
            _fileService = fileService;
            _printService = printService;
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _uriOpenerService = uriOpenerService;
            _ftpService = ftpService;
            _dialogService = dialogService;
            _localizationService = localizationService;
            _rdpService = rdpService;
            _wakeOnLanService = wakeOnLanService;

            ShowDetails = false;
            ShowMiscellaneous = true;
            ShowActions = true;
            SelectedDevice = null;
            ScannedDevices = new FilteredCollection<ScannedDevice>();

            _ipRangeModule = ipRangeModule;
            _progressModule = progressModule;
            _favoritesDevicesModule = favoritesDevicesModule;
            _searchModule = new SearchModule(ScannedDevices);
            _scanningModule = scanningModule;
            _scanningModule.InitializeCollection(ScannedDevices);

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

        public FilteredCollection<ScannedDevice> ScannedDevices
        {
            get => _filteredDevices;
            set => SetProperty(ref _filteredDevices, value);
        }

        public IpRangeModule IpRangeModule => _ipRangeModule;

        public SearchModule SearchModule => _searchModule;

        public ProgressModule ProgressModule => _progressModule;

        public ScanningModule ScanningModule => _scanningModule;

        public FavoritesDevicesModule FavoritesDevicesModule => _favoritesDevicesModule;

        public ICommand RightTappedCommand => new RelayCommand<ScannedDevice>(OnRightTapped);

        public ICommand SaveDeviceCommand => new AsyncRelayCommand(SaveDeviceAsync);

        public ICommand ExploreInExplorerCommand => new RelayCommand(ExploreInExplorerAsync);

        public ICommand ExploreHttpCommand => new AsyncRelayCommand(ExploreHttpAsync);

        public ICommand ExploreHttpsCommand => new AsyncRelayCommand(ExploreHttpsAsync);

        public ICommand ExploreFtpCommand => new AsyncRelayCommand(ExploreFtpAsync);

        public ICommand ExploreRdpCommand => new RelayCommand(ExploreRdp);

        public ICommand WakeOnLanCommand => new AsyncRelayCommand(WakeOnLanAsync);

        private void OnRightTapped(ScannedDevice selectedItem)
        {
            SelectedDevice = selectedItem;
        }

        private async Task SaveDeviceAsync()
        {
            StorageFile file = await _fileService.GetFileForWritingAsync(".json", ".xml", ".csv", ".html");
            if (file == null || SelectedDevice == null)
            {
                return;
            }

            IDeviceRepository repository = _deviceRepositoryFactory.CreateWithFile(file);
            await repository.SaveDevicesAsync(new List<ScannedDevice> { SelectedDevice });
        }

        private async void ExploreInExplorerAsync()
        {
            await _uriOpenerService.OpenUriAsync(new Uri($"file://{SelectedDevice.Ip}"));
        }

        private async Task ExploreHttpAsync()
        {
            await _uriOpenerService.OpenUriAsync(new Uri($"http://{SelectedDevice.Ip}"));
        }

        private async Task ExploreHttpsAsync()
        {
            await _uriOpenerService.OpenUriAsync(new Uri($"https://{SelectedDevice.Ip}"));
        }

        private async Task ExploreFtpAsync()
        {
            var ftpConfiguration = new FtpConfiguration($@"ftp://{SelectedDevice.Ip}", "anonymous", "anonymous");
            bool connected = await _ftpService.ConnectAsync(ftpConfiguration);

            if (connected)
            {
                string connectedMessage = _localizationService.GetLocalizedString("Connected");
                string successfullyConnectedMessage = _localizationService.GetLocalizedString("FtpSuccess");

                await _dialogService.ShowMessageAsync(connectedMessage, successfullyConnectedMessage);
            }
            else
            {
                string error = _localizationService.GetLocalizedString("Error");
                string couldNotConnectMessage = _localizationService.GetLocalizedString("FtpError");

                await _dialogService.ShowMessageAsync(error, couldNotConnectMessage);
            }
        }

        private void ExploreRdp()
        {
            try
            {
                _rdpService.Connect(new RdpConfiguration(SelectedDevice.Ip));
            }
            catch (Exception)
            {
                string error = _localizationService.GetLocalizedString("Error");
                string couldNotConnectMessage = _localizationService.GetLocalizedString("RdpError");

                _dialogService.ShowMessageAsync(error, couldNotConnectMessage);
            }
        }

        private async Task WakeOnLanAsync()
        {
            await _wakeOnLanService.SendPacketAsync(SelectedDevice.MacAddress, SelectedDevice.Ip);
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<FilterMessage>(this, OnFilterMessage);
            messenger.Register<DetailsPageVisibilityMessage>(this, OnDetailsPageVisibilityMessage);
            messenger.Register<MiscellaneousBarVisibilityMessage>(this, OnMiscellaneousBarVisibilityMessage);
            messenger.Register<ActionsBarVisibilityMessage>(this, OnActionsBarVisibilityMessage);
            messenger.Register<ScanFromFileMessage>(this, OnScanFromFileMessage);
            messenger.Register<PrintPreviewMessage>(this, OnPrintMessage);
        }

        private void OnFilterMessage(object sender, FilterMessage message)
        {
            ScannedDevices.ApplyFilter(message.FilterStatus, message.Filter);
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

        private void OnScanFromFileMessage(object sender, ScanFromFileMessage message)
        {
            IpRangeModule.IpRange = message.Content;
            ScanningModule.ScanCommand.Execute(this);
        }

        private async void OnPrintMessage(object sender, PrintPreviewMessage message)
        {
            IEnumerable<ScannedDevice> devices = FavoritesDevicesModule.DisplayFavorites 
                ? FavoritesDevicesModule.FavoritesDevices.FilteredItems
                : ScannedDevices.FilteredItems;

            await _printService.ShowPrintUIAsync(devices);
        }
    }
}
