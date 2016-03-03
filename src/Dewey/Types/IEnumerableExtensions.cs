using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Types
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            for (var i = 0; i < enumerable.Count(); i++) {
                var item = enumerable.ElementAt(i);

                action(item);
            }
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}
