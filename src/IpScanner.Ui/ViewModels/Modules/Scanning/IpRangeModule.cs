using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Ui.Services;

namespace IpScanner.Ui.ViewModels.Modules.Scanning
{
    public class IpRangeModule : ObservableObject
    {
        private string _ipRange;
        private readonly ValidationModule _validationModule;
        private readonly AppSettings _appSettings;

        public IpRangeModule(ISettingsService settingsService)
        {
            _appSettings = settingsService.Settings;
            _validationModule = new ValidationModule();

            IpRange = _appSettings.IpRange;
        }

        public string IpRange
        {
            get => _ipRange;
            set
            {
                _validationModule.HasValidationError = false;
                if(SetProperty(ref _ipRange, value))
                {
                    _appSettings.IpRange = value;
                }
            }
        }

        public ValidationModule ValidationModule { get => _validationModule; }

        public RelayCommand SetSubnetMask { get => new RelayCommand(EnableSubnetMask); }

        public RelayCommand SetSubnetClassCMask { get => new RelayCommand(EnableSubnetClassCMask); }

        private void EnableSubnetMask()
        {
            IpRange = "192.168.0.1-254, 26.0.0.1-254";
        }

        private void EnableSubnetClassCMask()
        {
            IpRange = "192.168.0.1-254";
        }
    }
}
