using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _mainFrame;

        public NavigationService(Frame frame)
        {
            _mainFrame = frame;
        }

        public void ChangeColorTheme(ElementTheme theme)
        {
            _mainFrame.RequestedTheme = theme;
        }

        public void NavigateToPage(Frame frame, Type typeOfPage)
        {
            frame.Navigate(typeOfPage);
        }

        public void ReloadMainPage()
        {
            _mainFrame.Navigate(_mainFrame.Content.GetType());
        }
    }
}
