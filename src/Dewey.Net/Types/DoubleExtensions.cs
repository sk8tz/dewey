using System;

namespace Dewey.Net.Types
{
    public static class DoubleExtensions
    {
        public static decimal ToDecimal(this double value)
        {
            return Convert.ToDecimal(value);
        }
    }
}
