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

        public void ReloadMainPage()
        {
            _mainFrame.Navigate(_mainFrame.Content.GetType());
        }
    }
}
