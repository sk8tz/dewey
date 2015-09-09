using System;

namespace Dewey.Net.Temporal
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static bool ThereAbouts(this DateTime value, DateTime other)
        {
            return value.Year == other.Year
                && value.Month == other.Month
                && value.Day == other.Day;
        }

        public static bool After(this DateTime value, DateTime other)
        {
            return value.CompareTo(other) > 0;
        }

        public static bool Before(this DateTime value, DateTime other)
        {
            return value.CompareTo(other) < 0;
        }

        public static DateTime StripTime(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        public static DateTime SetTime(this DateTime value, DateTime other)
        {
            return value.SetTime(other.Hour, other.Minute, other.Second);
        }

        public static DateTime SetTime(this DateTime value, Time other)
        {
            return value.SetTime(other.Hour, other.Minute, other.Second);
        }

        public static DateTime SetTime(this DateTime value, int hour, int minute, int second)
        {
            return new DateTime(value.Year, value.Month, value.Day, hour, minute, second);
        }

        public static long ToUnix(this DateTime value)
        {
            return (long)(value - UnixEpoch).TotalMilliseconds;
        }

        public static DateTime FromUnix(this long unix)
        {
            return UnixEpoch.AddMilliseconds(unix);
        }

        public static DateTime Merge(this DateTime value, TimeSpan timeSpan)
        {
            return value.Date + timeSpan;
        }

        public static DateTime Merge(this DateTime value, Time time)
        {
            return value.Date + time.ToTimeSpan();
        }

        public static bool IsMin(this DateTime value)
        {
            return (value == DateTime.MinValue);
        }
    }
}
