using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IpScanner.Ui.ObjectModels
{
    public class FilteredCollection<T> : ObservableCollection<T>
    {
        private ObservableCollection<T> _filteredItems;
        private ICollection<ItemFilter<T>> _filters;

        public FilteredCollection() : base()
        {
            _filteredItems = new ObservableCollection<T>();
            _filters = new List<ItemFilter<T>>();
        }

        public ObservableCollection<T> FilteredItems
        {
            get => _filteredItems;
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (ItemSutisfiesFilters(item))
            {
                _filteredItems.Add(item);
            }
        }

        public void AddFilter(ItemFilter<T> filter)
        {
            _filters.Add(filter);
        }

        public void RemoveFilter(ItemFilter<T> filter)
        {
            _filters.Remove(filter);
        }

        public void RefreshFilteredItems()
        {
            FilteredItems.Clear();

            foreach (var item in this)
            {
                if (ItemSutisfiesFilters(item))
                {
                    FilteredItems.Add(item);
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            FilteredItems.RemoveAt(index);
        }

        protected override void ClearItems()
        {
            _filteredItems.Clear();
            base.ClearItems();
        }

        private bool ItemSutisfiesFilters(T item)
        {
            foreach (var filter in _filters)
            {
                if (filter.Filter.Invoke(item) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
