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
            this.InitializePanelContainer();

            DataContext = Ioc.Default.GetService<ScanPageViewModel>();
            DetailsFrame.Navigate(typeof(DetailsPage));
        }

        private void InitializePanelContainer()
        {
            IPanelContainer panelContainer = Ioc.Default.GetService<IPanelContainer>();
            panelContainer.Inialize(CustomPrintContainer);
        }
    }
}
