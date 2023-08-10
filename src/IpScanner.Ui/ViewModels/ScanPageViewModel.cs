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
using IpScanner.Ui.ViewModels.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

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
        private readonly IPrintServiceFactory _printServiceFactory;
        private readonly IPrintElementRepository _printingElementRepository;
        private readonly IUriOpenerService _uriOpenerService;
        private readonly IFtpService _ftpService;
        private readonly IDialogService _dialogService;
        private readonly ILocalizationService _localizationService;

        public ScanPageViewModel(IMessenger messenger, IFileService fileService, IPrintServiceFactory printServiceFactory, 
            IDeviceRepositoryFactory deviceRepositoryFactory, FavoritesDevicesModule favoritesDevicesModule, ProgressModule progressModule,
            IpRangeModule ipRangeModule, ScanningModule scanningModule, IPrintElementRepository printingElementRepository, 
            IUriOpenerService uriOpenerService, IFtpService ftpService, IDialogService dialogService, ILocalizationService localizationService)
        {
            _messanger = messenger;
            _fileService = fileService;
            _printServiceFactory = printServiceFactory;
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _printingElementRepository = printingElementRepository;
            _uriOpenerService = uriOpenerService;
            _ftpService = ftpService;
            _dialogService = dialogService;
            _localizationService = localizationService;

            ShowDetails = false;
            ShowMiscellaneous = true;
            ShowActions = true;
            SelectedDevice = new ScannedDevice(System.Net.IPAddress.Any);
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

        public ICommand ExploreInExplorerCommand => new AsyncRelayCommand(ExploreInExplorerAsync);

        public ICommand ExploreHttpCommand => new AsyncRelayCommand(ExploreHttpAsync);

        public ICommand ExploreHttpsCommand => new AsyncRelayCommand(ExploreHttpsAsync);

        public ICommand ExploreFtpCommand => new AsyncRelayCommand(ExploreFtpAsync);

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

        private async Task ExploreInExplorerAsync()
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

        private async void OnScanFromFileMessage(object sender, ScanFromFileMessage message)
        {
            IpRangeModule.IpRange = message.Content;
            await ScanningModule.ScanCommand.ExecuteAsync(this);
        }

        private void OnPrintMessage(object sender, PrintPreviewMessage message)
        {
            FrameworkElement elementToPrint = _printingElementRepository.GetElementToPrint();
            if (elementToPrint == null)
            {
                throw new InvalidOperationException("Element to print is not initialized");
            }

            IPrintService printService = _printServiceFactory.CreateBasedOneFrameworkElement(elementToPrint);
            printService.ShowPrintUIAsync();
        }
    }
}
