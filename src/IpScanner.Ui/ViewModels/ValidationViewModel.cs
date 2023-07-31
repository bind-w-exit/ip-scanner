using CommunityToolkit.Mvvm.ComponentModel;

namespace IpScanner.Ui.ViewModels
{
    public abstract class ValidationViewModel : ObservableValidator
    {
        private bool _hasValidationError;

        public ValidationViewModel()
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
