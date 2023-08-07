using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ScanPage : Page
    {
        public ScanPage()
        {
            this.InitializeComponent();
            DataContext = GetAndInitializeScanPage();

            DetailsFrame.Navigate(typeof(DetailsPage));
        }

        private ScanPageViewModel GetAndInitializeScanPage()
        {
            var viewModel = Ioc.Default.GetService<ScanPageViewModel>();
            viewModel.InitializeElementToPrint(ScannedDevicesDataGrid);

            return viewModel;
        }
    }
}
