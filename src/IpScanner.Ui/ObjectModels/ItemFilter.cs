using System;

namespace IpScanner.Ui.ObjectModels
{
    public class ItemFilter<T> : IEquatable<ItemFilter<T>>
    {
        private readonly Guid _id;

        public ItemFilter(Func<T, bool> filter)
        {
            _id = Guid.NewGuid();
            Filter = filter;
        }

        public Func<T, bool> Filter { get; }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ItemFilter<T> other)
        {
            return _id == other._id;
        }
    }
}
