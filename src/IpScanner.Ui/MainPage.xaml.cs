using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Pages;
using IpScanner.Ui.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;
using IpScanner.Ui.Services;

namespace IpScanner.Ui
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeScanPageViewModelForMessanger();
            this.CustomizeToolbar();

            DataContext = Ioc.Default.GetService<MainPageViewModel>();
            ContentFrame.Navigate(typeof(ScanPage));
        }

        private void InitializeScanPageViewModelForMessanger() => Ioc.Default.GetService<ScanPageViewModel>();

        private void CustomizeToolbar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var contentBarService = new ContentBarCustomizationService(coreTitleBar, AppTitleBar, LeftPaddingColumn, RightPaddingColumn);
            contentBarService.Customize();
        }
    }
}
