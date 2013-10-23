using System;

namespace Impromptu
{
    public static class Extensions
    {

        #region Public Static Methods

        public static double NextDouble(this Random random, double min, double max)
        {
            return min + (random.NextDouble() * (max - min));
        }

        public static byte[] ToByteArray(this string value)
        {
            var encoding = new System.Text.UTF8Encoding();
            return value != null ? encoding.GetBytes(value) : null;
        }

        public static bool Equals(this DateTime dateTime1, DateTime dateTime2)
        {
            const int EPSILON = 1000;
            var result = System.Math.Abs(System.Math.Abs((dateTime1 - dateTime2).TotalMilliseconds)) < EPSILON;
            return result;
        }

        #endregion

    }
}
