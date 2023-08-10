using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class IpRangeModule : ObservableObject
    {
        private string _ipRange;
        private readonly ValidationModule _validationModule;

        public IpRangeModule()
        {
            _validationModule = new ValidationModule();

            IpRange = string.Empty;
        }

        public string IpRange
        {
            get => _ipRange;
            set
            {
                _validationModule.HasValidationError = false;
                SetProperty(ref _ipRange, value);
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
