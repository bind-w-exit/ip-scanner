using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            this.InitializeComponent();

            DataContext = Ioc.Default.GetService<DetailsPageViewModel>();
        }
    }
}
