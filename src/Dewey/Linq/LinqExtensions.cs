using System;

namespace Dewey.Linq
{
    public static class LinqExtensions
    {
        public static Func<T, bool> And<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2) => (arg => predicate1(arg) && predicate2(arg));

        public static Func<T, bool> Or<T>(this Func<T, bool> predicate1, Func<T, bool> predicate2) => (arg => predicate1(arg) || predicate2(arg));
    }
}
