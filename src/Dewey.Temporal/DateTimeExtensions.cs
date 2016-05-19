using System;

namespace Dewey.Temporal
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static bool ThereAbouts(this DateTime value, DateTime other) => (value.Year == other.Year && value.Month == other.Month && value.Day == other.Day);

        public static bool After(this DateTime value, DateTime other) => value.CompareTo(other) > 0;

        public static bool Before(this DateTime value, DateTime other) => value.CompareTo(other) < 0;

        public static DateTime StripTime(this DateTime value) => new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);

        public static DateTime SetTime(this DateTime value, DateTime other) => value.SetTime(other.Hour, other.Minute, other.Second);

        public static DateTime SetTime(this DateTime value, Time other) => value.SetTime(other.Hour, other.Minute, other.Second);

        public static DateTime SetTime(this DateTime value, int hour, int minute, int second) => new DateTime(value.Year, value.Month, value.Day, hour, minute, second);

        public static long ToUnix(this DateTime value) => (long)(value - UnixEpoch).TotalMilliseconds;

        public static DateTime FromUnix(this long unix) => UnixEpoch.AddMilliseconds(unix);

        public static DateTime Merge(this DateTime value, TimeSpan timeSpan) => (value.Date + timeSpan);

        public static DateTime Merge(this DateTime value, Time time) => (value.Date + time.ToTimeSpan());

        public static bool IsMin(this DateTime value) => (value == DateTime.MinValue);

        public static string ToUniversalDateTimeString(this DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ss zzz");
    }
}
