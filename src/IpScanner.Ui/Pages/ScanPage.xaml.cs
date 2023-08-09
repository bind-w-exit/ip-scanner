using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Printing;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ScanPage : Page
    {
        public ScanPage()
        {
            this.InitializeComponent();

            PreparePrintingElementRepo(Ioc.Default.GetService<IPrintElementRepository>());

            DataContext = Ioc.Default.GetService<ScanPageViewModel>();
            DetailsFrame.Navigate(typeof(DetailsPage));
        }

        private void PreparePrintingElementRepo(IPrintElementRepository elementRepository)
        {
            elementRepository.ClearElements();
            elementRepository.AddElements(DevicesGrid);
        }
    }
}
