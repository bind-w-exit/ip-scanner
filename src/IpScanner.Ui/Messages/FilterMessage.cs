using IpScanner.Domain.Models;
using IpScanner.Ui.ObjectModels;

namespace IpScanner.Ui.Messages
{
    public class FilterMessage : FilterMessageBase<ScannedDevice>
    {
        public FilterMessage(ItemFilter<ScannedDevice> filter, bool filterStatus) : base(filter, filterStatus)
        { }
    }
}
