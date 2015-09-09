using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dewey.Net.List
{
    public static class ListExtensions
    {
        public static List<T> Fill<T>(int size, T value)
        {
            var result = new List<T>();

            for (var i = 0; i < size; ++i) {
                result.Add(value);
            }

            return result;
        }
    }
}