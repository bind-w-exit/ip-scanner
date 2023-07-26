using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Implementations
{
    public class LazyListViewProvider : ILazyResultProvider
    {
        private readonly ListView _listView;

        public LazyListViewProvider(ListView listView)
        {
            _listView = listView;
        }

        public void Report(ScannedDevice device, int progress)
        {
            _listView.Items.Add(device);
        }
    }
}
