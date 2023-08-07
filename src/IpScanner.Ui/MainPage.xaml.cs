using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Pages;
using IpScanner.Ui.Printing;
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

        private void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var printHelper = new PrintService(ContentFrame);
            printHelper.ShowPrintUIAsync();
        }
    }
}
