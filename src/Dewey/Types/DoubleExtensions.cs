using System;

namespace Dewey.Types
{
    public static class DoubleExtensions
    {
        public static decimal ToDecimal(this double value) => Convert.ToDecimal(value);
    }
}
