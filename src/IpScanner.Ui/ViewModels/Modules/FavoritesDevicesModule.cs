using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Ui.ObjectModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class FavoritesDevicesModule : ObservableObject
    {
        private bool _displayFavorites;
        private readonly IDeviceRepository _deviceRepository;
        private readonly FilteredCollection<ScannedDevice> _filteredDevices;

        public FavoritesDevicesModule(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
            _displayFavorites = false;
            _filteredDevices = new FilteredCollection<ScannedDevice>();
        }

        public FilteredCollection<ScannedDevice> FavoritesDevices { get => _filteredDevices; }

        public bool DisplayFavorites
        {
            get => _displayFavorites;
            set => SetProperty(ref _displayFavorites, value);
        }

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadFavorites); }

        public RelayCommand UnloadFavoritesCommand { get => new RelayCommand(UnloadFavorites); }

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
    }
}
