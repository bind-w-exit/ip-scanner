using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace IpScanner.Ui.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ObservableCollection<T>(source);
        }
    }
}
