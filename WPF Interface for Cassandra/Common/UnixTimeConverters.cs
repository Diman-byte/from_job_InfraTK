using System;

namespace Common
{
    public static class UnixTimeConverters
    {
        /// <summary>
        /// Получить Unix из DateTime
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>Unix</returns>
        public static uint ConvertToUnixTimes(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0);
            var diff = date - origin;
            return (uint)diff.TotalSeconds;
        }

        /// <summary>
        /// Получить DateTime из Unix
        /// </summary>
        /// <param name="timestamp">Unix Time</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertFromUnixTimes(ulong timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}

