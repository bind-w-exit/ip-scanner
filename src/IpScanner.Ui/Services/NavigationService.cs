using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void ReloadMainPage()
        {
            _frame.Navigate(_frame.Content.GetType());
        }
    }
}
