namespace Dewey.Net.Types
{
    public static class LongExtensions
    {
        public static bool After(this long value, long other)
        {
            return (value > other);
        }

        public static bool Before(this long value, long other)
        {
            return (value < other);
        }
    }
}
