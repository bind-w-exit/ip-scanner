using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Exceptions;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class FavoritesDevicesModule : ObservableObject
    {
        private bool _displayFavorites;
        private string _selectedCollection;
        private StorageFile _storageFile;
        private ScannedDevice _selectedDevice;
        private readonly ObservableCollection<string> _collections;
        private readonly FilteredCollection<ScannedDevice> _filteredDevices;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;

        public FavoritesDevicesModule(IMessenger messenger, IFileService fileService, 
            IDeviceRepositoryFactory deviceRepositoryFactory, IDialogService dialogService)
        {
            _fileService = fileService;
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _dialogService = dialogService;

            _displayFavorites = false;
            _filteredDevices = new FilteredCollection<ScannedDevice>();
            _collections = new ObservableCollection<string>
            {
                "Results",
                "Favorites"
            };

            _selectedCollection = _collections.First();

            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
            messenger.Register<DevicesLoadedMessage>(this, OnDevicesLoaded);
        }

        public FilteredCollection<ScannedDevice> FavoritesDevices { get => _filteredDevices; }

        public bool DisplayFavorites
        {
            get => _displayFavorites;
            set => SetProperty(ref _displayFavorites, value);
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

        public ObservableCollection<string> Collections => _collections;

        public AsyncRelayCommand<string> ExecuteSelectedOption => new AsyncRelayCommand<string>(ExecuteOptionAsync);

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadFavoritesAsync); }

        public AsyncRelayCommand UnloadFavoritesCommand { get => new AsyncRelayCommand(UnloadFavoritesAsync); }

        public RelayCommand AddToFavoritesCommand { get => new RelayCommand(AddToFavorites); }

        public RelayCommand RemoveFromFavoritesCommand { get => new RelayCommand(RemoveFromFavorites); }

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
                await UnloadFavoritesCommand.ExecuteAsync(null);
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
            try
            {
                StorageFile file = await GetStorageFileAsync();
                IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

                List<ScannedDevice> devices = (await deviceRepository.GetDevicesAsync()).ToList();
                foreach (var device in devices)
                {
                    FavoritesDevices.Add(device);
                }

                DisplayFavorites = true;
            }
            catch (ContentFormatException)
            {
                await _dialogService.ShowMessageAsync("Error", "The file is corrupted. Please delete it and try again.");
            }
        }

        private async Task UnloadFavoritesAsync()
        {
            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

            await deviceRepository.SaveDevicesAsync(FavoritesDevices);
            DisplayFavorites = false;
            FavoritesDevices.Clear();
        }

        private void AddToFavorites()
        {
            if (_selectedDevice == null)
            {
                return;
            }

            _selectedDevice.MarkAsFavorite();
            FavoritesDevices.Add(_selectedDevice);
        }

        private void RemoveFromFavorites()
        {
            if (_selectedDevice == null)
            {
                return;
            }

            _selectedDevice.UnmarkAsFavorite();
            FavoritesDevices.Remove(_selectedDevice);
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            _selectedDevice = message.Device;
        }
    }
}
