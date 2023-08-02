using IpScanner.Domain.Models;
using IpScanner.Ui.ObjectModels;

namespace IpScanner.Ui.Messages
{
    public class FilterMessage
    {
        public FilterMessage(ItemFilter<ScannedDevice> filter, bool filterStatus)
        {
            Filter = filter;
            FilterStatus = filterStatus;
        }

        public ItemFilter<ScannedDevice> Filter { get; }
        public bool FilterStatus { get; }
    }
}
