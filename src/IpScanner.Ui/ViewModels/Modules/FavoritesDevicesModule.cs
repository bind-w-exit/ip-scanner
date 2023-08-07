using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class FavoritesDevicesModule : ObservableObject
    {
        private bool _displayFavorites;
        private ScannedDevice _selectedDevice;
        private readonly IFileService _fileService;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;
        private readonly FilteredCollection<ScannedDevice> _filteredDevices;

        public FavoritesDevicesModule(IMessenger messenger, IFileService fileService, IDeviceRepositoryFactory deviceRepositoryFactory)
        {
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _fileService = fileService;

            _displayFavorites = false;
            _filteredDevices = new FilteredCollection<ScannedDevice>();

            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
        }

        public FilteredCollection<ScannedDevice> FavoritesDevices { get => _filteredDevices; }

        public bool DisplayFavorites
        {
            get => _displayFavorites;
            set => SetProperty(ref _displayFavorites, value);
        }

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadFavoritesAsync); }

        public AsyncRelayCommand UnloadFavoritesCommand { get => new AsyncRelayCommand(UnloadFavoritesAsync); }

        public RelayCommand AddToFavoritesCommand { get => new RelayCommand(AddToFavorites); }

        public RelayCommand RemoveFromFavoritesCommand { get => new RelayCommand(RemoveFromFavorites); }

        private async Task LoadFavoritesAsync()
        {
            StorageFile file = await _fileService.GetDefaultFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

            List<ScannedDevice> devices = (await deviceRepository.GetDevicesAsync()).ToList();
            foreach (var device in devices)
            {
                FavoritesDevices.Add(device);
            }

            DisplayFavorites = true;
        }

        private async Task UnloadFavoritesAsync()
        {
            StorageFile file = await _fileService.GetDefaultFileAsync();
            IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);

            await deviceRepository.SaveDevicesAsync(FavoritesDevices);
            DisplayFavorites = false;
            FavoritesDevices.Clear();
        }

        private void AddToFavorites()
        {
            if(_selectedDevice == null)
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
