using Dewey.Net.Log;
using System;

namespace Dewey.Net.Temporal
{
    public class DateTimeUtils
    {
        public static long GetValue(DateTime date)
        {
            var Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(date - Jan1st1970).TotalMilliseconds;
        }

        public static DateTime FromMillis(long ticks, bool ignoreError = true)
        {
            if (ticks == 0) {
                return DateTime.MinValue;
            }

            var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

            return posixTime.AddMilliseconds(ticks);
        }

        public static DateTime FromMillis(string ticks, bool ignoreError = true)
        {
            long millis;

            if (!long.TryParse(ticks, out millis)) {
                return DateTime.Now;
            }

            var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

            return posixTime.AddMilliseconds(millis);
        }

        public static DateTime Time(int hour, int minute, int second)
        {
            return new DateTime(1970, 1, 1, hour, minute, second);
        }

        public static DateTime Merge(DateTime dateTime, TimeSpan timeSpan)
        {
            return dateTime.Date + timeSpan;
        }
    }
}