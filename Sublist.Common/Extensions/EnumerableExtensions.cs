using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sublist.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in enumerable)
            {
                result.Add(item);
            }
            return result;
        }
    }
}