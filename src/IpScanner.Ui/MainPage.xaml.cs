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
            InitializeScanPageViewModelForMessanger();

            DataContext = Ioc.Default.GetService<MainPageViewModel>();

            ContentFrame.Navigate(typeof(ScanPage));
        }

        private void InitializeScanPageViewModelForMessanger() => Ioc.Default.GetService<ScanPageViewModel>();
    }
}
