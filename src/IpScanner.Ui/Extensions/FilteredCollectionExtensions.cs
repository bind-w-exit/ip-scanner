using IpScanner.Ui.ObjectModels;

namespace IpScanner.Ui.Extensions
{
    public static class FilteredCollectionExtensions
    {
        public static void ApplyFilter<T>(this FilteredCollection<T> collection, bool filterStatus, ItemFilter<T> filter)
        {
            if (filterStatus)
            {
                collection.AddFilter(filter);
            }
            else
            {
                collection.RemoveFilter(filter);
            }

            collection.RefreshFilteredItems();
        }
    }
}
