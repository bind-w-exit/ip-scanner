using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Services
{
    public interface INavigationService
    {
        void ChangeColorTheme(ElementTheme theme);
        void ReloadMainPage();
        void NavigateToPage(Frame frame, Type typeOfPage);
    }
}
