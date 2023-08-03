using CommunityToolkit.Mvvm.ComponentModel;

namespace IpScanner.Ui.ViewModels.Modules
{
    public class ValidationModule : ObservableObject
    {
        private bool _hasValidationError;

        public ValidationModule()
        {
            _hasValidationError = false;
        }

        public bool HasValidationError
        {
            get => _hasValidationError;
            set => SetProperty(ref _hasValidationError, value);
        }
    }
}
