using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Extensions;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace IpScanner.Ui.ViewModels.Modules.Scanning
{
    public class FavoritesDevicesModule : ObservableObject
    {
        private bool _displayFavorites;
        private string _selectedCollection;
        private StorageFile _storageFile;
        private ScannedDevice _selectedDevice;
        private readonly ObservableCollection<string> _collections;
        private readonly FilteredCollection<ScannedDevice> _favoriteDevices;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;
        private readonly ILocalizationService _localizationService;

        public FavoritesDevicesModule(IMessenger messenger, IFileService fileService, IDeviceRepositoryFactory deviceRepositoryFactory, 
            IDialogService dialogService, ILocalizationService localizationService, ISettingsService settingsService)
        {
            _appSettings = settingsService.Settings;
            _fileService = fileService;
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _dialogService = dialogService;
            _localizationService = localizationService;

            DisplayFavorites = _appSettings.FavoritesSelected;
            _collections = new ObservableCollection<string>
            {
                "Results",
                "Favorites"
            };
            SelectedCollection = _appSettings.FavoritesSelected ? "Favorites" : "Results";
            _favoriteDevices = new FilteredCollection<ScannedDevice>();

            RegisterMessages(messenger);
        }

        public bool DisplayFavorites
        {
            get => _displayFavorites;
            set
            {
                if(SetProperty(ref _displayFavorites, value))
                {
                    _appSettings.FavoritesSelected = value;
                }
            }
        }

        public string SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                SetProperty(ref _selectedCollection, value);
                ExecuteSelectedOption.Execute(value);
            }
        }

        public FilteredCollection<ScannedDevice> FavoritesDevices { get => _favoriteDevices; }

        public ICommand ExecuteSelectedOption => new AsyncRelayCommand<string>(ExecuteOptionAsync);

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadFavoritesAsync); }

        public ICommand UnloadFavoritesCommand { get => new RelayCommand(UnloadFavorites); }

        public AsyncRelayCommand AddToFavoritesCommand { get => new AsyncRelayCommand(AddToFavoritesAsync); }

        public AsyncRelayCommand RemoveFromFavoritesCommand { get => new AsyncRelayCommand(RemoveFromFavoritesAsync); }

        public void SetStorageFile(StorageFile file)
        {
            _storageFile = file;
        }

        private async Task<StorageFile> GetStorageFileAsync()
        {
            if (_storageFile == null)
            {
                _storageFile = await _fileService.GetDefaultFileAsync();
            }

            return _storageFile;
        }

        private async Task ExecuteOptionAsync(string collection)
        {
            if (collection == "Results")
            {
                UnloadFavoritesCommand.Execute(null);
            }
            else if (collection == "Favorites")
            {
                await LoadFavoritesCommand.ExecuteAsync(null);
            }
        }

        private void OnDevicesLoaded(object sender, DevicesLoadedMessage message)
        {
            SetStorageFile(message.StorageFile);

            FavoritesDevices.Clear();
            SelectedCollection = _collections.First(x => x == "Favorites");
        }

        private async Task LoadFavoritesAsync()
        {
            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);
            
            IEnumerable<ScannedDevice> items = await deviceRepository.GetDevicesOrNullAsync();
            if (items == null)
            {
                await ShowLoadingDataErrorMessage();
                return;
            }

            List<ScannedDevice> devices = items.ToList();
            foreach (var device in devices)
            {
                FavoritesDevices.Add(device);
            }

            DisplayFavorites = true;
        }

        private void UnloadFavorites()
        {
            DisplayFavorites = false;
            FavoritesDevices.Clear();
        }

        private async Task AddToFavoritesAsync()
        {
            if (_selectedDevice == null)
            {
                return;
            }

            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

            _selectedDevice.MarkAsFavorite();
            await deviceRepository.AddDeviceAsync(_selectedDevice);
        }

        private async Task RemoveFromFavoritesAsync()
        {
            if (_selectedDevice == null)
            {
                return;
            }

            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

            _selectedDevice.UnmarkAsFavorite();
            await deviceRepository.RemoveDeviceAsync(_selectedDevice);
            FavoritesDevices.Remove(_selectedDevice);
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            _selectedDevice = message.Device;
        }

        private void OnFilterMessage(object sender, FilterMessage message)
        {
            FavoritesDevices.ApplyFilter(message.FilterStatus, message.Filter);
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
            messenger.Register<DevicesLoadedMessage>(this, OnDevicesLoaded);
            messenger.Register<FilterMessage>(this, OnFilterMessage);
        }

        private async Task ShowLoadingDataErrorMessage()
        {
            string title = _localizationService.GetLocalizedString("Error");
            string message = _localizationService.GetLocalizedString("CorruptedMessage");

            await _dialogService.ShowMessageAsync(title, message);
        }
    }
}
