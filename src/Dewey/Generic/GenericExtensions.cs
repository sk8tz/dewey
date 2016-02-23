using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Generic
{
    public static class GenericExtensions
    {
        public static IList<T> ToIList<T>(this IEnumerable<T> value) => value.ToList();
        
        public static bool IsEmpty<T>(this IEnumerable<T> value) => (value == null || value.Count() == 0);

        public static bool IsNotEmpty<T>(this IEnumerable<T> value) => !value.IsEmpty();

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var keys = new HashSet<TKey>();

            return source.Where(element => keys.Add(selector(element)));
        }
    }
}
