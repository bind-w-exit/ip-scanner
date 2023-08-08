using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Domain.Models;
using IpScanner.Ui.ObjectModels;
using System;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class SearchModule : ObservableObject
    {
        private string _searchText;
        private readonly ItemFilter<ScannedDevice> _searchFilter;
        private readonly FilteredCollection<ScannedDevice> _scannedDevices;

        public SearchModule(FilteredCollection<ScannedDevice> scannedDevices)
        {
            SearchText = string.Empty;

            _scannedDevices = scannedDevices;
            _searchFilter = new ItemFilter<ScannedDevice>(device => device.Name.Contains(SearchText, 
                StringComparison.OrdinalIgnoreCase));
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                bool isValueSet = SetProperty(ref _searchText, value);
                if (!isValueSet || _scannedDevices == null)
                    return;

                UpdateScannedDevicesSearchFilter();
            }
        }

        private void UpdateScannedDevicesSearchFilter()
        {
            _scannedDevices.RemoveFilter(_searchFilter);
            _scannedDevices.AddFilter(_searchFilter);
            _scannedDevices.RefreshFilteredItems();
        }
    }
}
