using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Pages;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = Ioc.Default.GetService<MainPageViewModel>();

            ContentFrame.Navigate(typeof(ScanPage));
        }
    }
}
