using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Impromptu.Utilities
{
    public static class Extensions
    {
        #region Public Static Methods

        /// <summary>
        /// Change type cast
        /// </summary>
        /// <param name="item">item to change</param>
        /// <typeparam name="T">change type to</typeparam>
        /// <remarks>Result of wanted to do dynamic cast to type of object</remarks>
        public static T Cast<T>(this object item)
        {
            return (T)Convert.ChangeType(item, typeof(T));
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return min + (random.NextDouble() * (max - min));
        }

        public static byte[] ToByteArray(this string value)
        {
            var encoding = new System.Text.UTF8Encoding();
            return value != null ? encoding.GetBytes(value) : null;
        }

        public static List<T> ToList<T>(this ConcurrentBag<T> bag)
        {
            var result = new List<T>();
            foreach(var item in bag)
                result.Add(item);
            return result.ToList();
        }

        public static DateTime UTCToLocal(this DateTime utc)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.Local);
            return localTime;
        }

        public static int CompareTo(this DateTime dateTime1, DateTime dateTime2, int epsilon)
        {
            var t1 = dateTime1;
            var t2 = dateTime2;

            var result = 0;
            if(System.Math.Abs(System.Math.Abs((t1 - t2).TotalMilliseconds)) < epsilon)
                result = -1;
            else if(System.Math.Abs(System.Math.Abs((t1 - t2).TotalMilliseconds)) > epsilon)
                result = 1;
            return result;
        }

        public static DateTime Closest(this DateTime[] datetimes, DateTime nearest)
        {
            return datetimes.OrderBy(t => System.Math.Abs((t - nearest).Ticks)).FirstOrDefault();
        }

        #endregion
    }
}
