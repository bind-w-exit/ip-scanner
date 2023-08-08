using System;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace IpScanner.Ui.Services
{
    public class ModalsService : IModalsService
    {
        private readonly IColorThemeService _colorThemeService;

        public ModalsService(IColorThemeService colorThemeService)
        {
            _colorThemeService = colorThemeService;
        }

        public async Task ShowPageAsync(Type pageType)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();

            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(pageType);

            RegisterFrameForThemeChanging(appWindowContentFrame);

            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            await appWindow.TryShowAsync();
        }

        private void RegisterFrameForThemeChanging(Frame frame)
        {
            _colorThemeService.Register(frame);
            _colorThemeService.SetColorTheme(frame, _colorThemeService.GetColorTheme());
        }
    }
}
