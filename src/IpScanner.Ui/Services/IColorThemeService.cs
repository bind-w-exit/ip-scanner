using Windows.UI.Xaml;

namespace IpScanner.Ui.Services
{
    public interface IColorThemeService
    {
        ElementTheme GetColorTheme();
        void SetColorTheme(ElementTheme theme);
        void Register(FrameworkElement element);
        void SetColorTheme(FrameworkElement element, ElementTheme theme);
    }
}
