using IpScanner.Infrastructure;
using IpScanner.Infrastructure.APIs;
using IpScanner.Ui.Implementations;
using System.Net;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui
{
    public sealed partial class MainPage : Page
    {
        private readonly LazyListViewProvider _lazyListViewProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MainPage()
        {
            this.InitializeComponent();

            _lazyListViewProvider = new LazyListViewProvider(DevicesListView);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async void ScanButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var startWith = IPAddress.Parse("192.168.0.103");
            var endWith = IPAddress.Parse("192.168.0.108");

            var scanner = Domain.Models.IpScanner.Create(startWith, endWith, _lazyListViewProvider,
                new ManufactorApi(), new ArpMacAddressScanner());

            await scanner.StartAsync(_cancellationTokenSource.Token);
        }

        private void StopButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
