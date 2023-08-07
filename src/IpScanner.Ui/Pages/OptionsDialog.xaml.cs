using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class OptionsDialog : ContentDialog
    {
        public OptionsDialog()
        {
            this.InitializeComponent();

            INavigationService navigationService = Ioc.Default.GetService<INavigationService>();
            DataContext = new OptionsDialogViewModel(OptionsFrame, navigationService);
        }
    }
}
