namespace Dewey.Types
{
    public static class LongExtensions
    {
        public static bool After(this long value, long other) => (value > other);

        public static bool Before(this long value, long other) => (value < other);
    }
}
