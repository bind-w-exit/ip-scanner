using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Services
{
    public class ColorThemeService : IColorThemeService
    {
        private readonly List<FrameworkElement> _registeredElements;

        public ColorThemeService(Frame mainFrame)
        {
            _registeredElements = new List<FrameworkElement> { mainFrame };
        }

        public ElementTheme GetColorTheme()
        {
            return _registeredElements.First().RequestedTheme;
        }

        public void Register(FrameworkElement element)
        {
            _registeredElements.Add(element);
        }

        public void SetColorTheme(ElementTheme theme)
        {
            foreach (var element in _registeredElements)
            {
                element.RequestedTheme = theme;
            }
        }

        public void SetColorTheme(FrameworkElement element, ElementTheme theme)
        {
            element.RequestedTheme = theme;
        }
    }
}
