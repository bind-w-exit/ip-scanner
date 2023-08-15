using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace IpScanner.Ui.Pages
{
    public sealed partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            this.InitializeComponent();

            var navigationService = Ioc.Default.GetService<INavigationService>();
            DataContext = new OptionsPageViewModel(ContentFrame, navigationService);
        }
    }
}
