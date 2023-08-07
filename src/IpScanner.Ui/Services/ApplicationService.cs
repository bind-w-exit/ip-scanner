using Windows.UI.Xaml;

namespace IpScanner.Ui.Services
{
    public class ApplicationService : IApplicationService
    {
        public void Exit()
        {
            Application.Current.Exit();
        }
    }
}
