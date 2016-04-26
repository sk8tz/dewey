using System;

namespace Dewey.Types
{
    public static class LongExtensions
    {
        public static bool IsAfter(this long value, long other) => (value > other);
        public static bool IsBefore(this long value, long other) => (value < other);

        public static decimal ToDecimal(this long value) => Convert.ToDecimal(value);
        public static double ToDouble(this long value) => Convert.ToDouble(value);
        public static int ToInt(this long value) => Convert.ToInt32(value);

        public static bool IsZero(this long value) => (value == 0);

        public static long Round(this long value, int decimals = 2) => Math.Round(value.ToDouble(), decimals).ToLong();

        public static long Add(this long value, long arg) => (value + arg);
        public static long Add(this long value, int arg) => (value + arg.ToLong());
        public static long Add(this long value, decimal arg) => (value + arg.ToLong());
        public static long Add(this long value, double arg) => (value + arg.ToLong());

        public static long Subtract(this long value, long arg) => (value - arg);
        public static long Subtract(this long value, decimal arg) => (value - arg.ToLong());
        public static long Subtract(this long value, double arg) => (value - arg.ToLong());
        public static long Subtract(this long value, int arg) => (value - arg.ToLong());

        public static long Multiply(this long value, long arg) => (value * arg);
        public static long Multiply(this long value, decimal arg) => (value * arg.ToLong());
        public static long Multiply(this long value, double arg) => (value * arg.ToLong());
        public static long Multiply(this long value, int arg) => (value * arg.ToLong());

        public static long Divide(this long value, long arg) => (value / arg);
        public static long Divide(this long value, decimal arg) => (value / arg.ToLong());
        public static long Divide(this long value, double arg) => (value / arg.ToLong());
        public static long Divide(this long value, int arg) => (value / arg.ToLong());
    }
}
