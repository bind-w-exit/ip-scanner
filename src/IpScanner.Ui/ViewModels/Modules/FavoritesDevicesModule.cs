using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class FavoritesDevicesModule : ObservableObject
    {
        private bool _displayFavorites;
        private ScannedDevice _selectedDevice;
        private readonly IDeviceRepository _deviceRepository;
        private readonly FilteredCollection<ScannedDevice> _filteredDevices;

        public FavoritesDevicesModule(IDeviceRepository deviceRepository, IMessenger messenger)
        {
            _deviceRepository = deviceRepository;
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

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadFavorites); }

        public RelayCommand UnloadFavoritesCommand { get => new RelayCommand(UnloadFavorites); }

        public AsyncRelayCommand AddToFavoritesCommand { get => new AsyncRelayCommand(AddToFavorites); }

        private async Task LoadFavorites()
        {
            DisplayFavorites = true;

            IEnumerable<ScannedDevice> devices = await _deviceRepository.GetDevicesAsync();
            foreach (var device in devices)
            {
                FavoritesDevices.Add(device);
            }
        }

        private void UnloadFavorites()
        {
            DisplayFavorites = false;
            FavoritesDevices.Clear();
        }

        private async Task AddToFavorites()
        {
            await _deviceRepository.AddDeviceAsync(_selectedDevice);
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            _selectedDevice = message.Device;
        }
    }
}
