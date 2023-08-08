using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            this.InitializeComponent();

            RegisterOptionsFrame();

            var navigationService = Ioc.Default.GetService<INavigationService>();
            DataContext = new OptionsPageViewModel(ContentFrame, navigationService);
        }

        private void RegisterOptionsFrame()
        {
            var colorThemeService = Ioc.Default.GetService<IColorThemeService>();

            colorThemeService.Register(OptionsFrame);
            colorThemeService.SetColorTheme(OptionsFrame, colorThemeService.GetColorTheme());
        }
    }
}
