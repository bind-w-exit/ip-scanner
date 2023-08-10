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

            InitializeScanPageViewModelForMessanger();
            DataContext = Ioc.Default.GetService<MainPageViewModel>();
            ContentFrame.Navigate(typeof(ScanPage));

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var contentBarService = new ContentBarCustomizationService(coreTitleBar, AppTitleBar, LeftPaddingColumn, RightPaddingColumn);

            contentBarService.Customize();
        }

        private void InitializeScanPageViewModelForMessanger() => Ioc.Default.GetService<ScanPageViewModel>();
    }
}
