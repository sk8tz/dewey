using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Net.Types
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
    }
}
