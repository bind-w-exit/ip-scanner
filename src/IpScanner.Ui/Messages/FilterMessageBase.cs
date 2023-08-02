using IpScanner.Domain.Models;
using IpScanner.Ui.ObjectModels;

namespace IpScanner.Ui.Messages
{
    public abstract class FilterMessageBase<T>
    {
        protected FilterMessageBase(ItemFilter<T> filter, bool filterStatus)
        {
            Filter = filter;
            FilterStatus = filterStatus;
        }

        public ItemFilter<T> Filter { get; }
        public bool FilterStatus { get; }
    }
}
