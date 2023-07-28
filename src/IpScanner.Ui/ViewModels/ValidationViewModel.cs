using CommunityToolkit.Mvvm.ComponentModel;

namespace IpScanner.Ui.ViewModels
{
    public abstract class ValidationViewModel : ObservableValidator
    {
        private string _validationMessage;

        public ValidationViewModel()
        {
            _validationMessage = string.Empty;
        }

        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }
    }
}
