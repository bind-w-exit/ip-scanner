using IpScanner.Ui.Implementations;
using System.Net;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui
{
    public sealed partial class MainPage : Page
    {
        private readonly LazyListViewProvider _lazyListViewProvider;

        public MainPage()
        {
            this.InitializeComponent();
            _lazyListViewProvider = new LazyListViewProvider(DevicesListView);
        }

        private async void ScanButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var startWith = IPAddress.Parse("192.168.0.103");
            var endWith = IPAddress.Parse("192.168.0.105");

            var scanner = Domain.Models.IpScanner.Create(startWith, endWith, _lazyListViewProvider);
            await scanner.StartAsync(new System.Threading.CancellationToken());
        }
    }
}
