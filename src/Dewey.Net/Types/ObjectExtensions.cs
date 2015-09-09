namespace Dewey.Net.Types
{
    public static class ObjectExtensions
    {
        public static bool IsBlank(this object value)
        {
            return value == null || value.ToString().IsEmpty();
        }

        public static bool IsNotBlank(this object value)
        {
            return !value.IsBlank();
        }

        public static T Default<T>(this object value, T defaultValue = default(T))
        {
            if (value.IsBlank()) {
                return defaultValue;
            }

            return (T)value;
        }
    }
}
