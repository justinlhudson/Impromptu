using System.Collections.Generic;
using System.Linq;
using System;

namespace Impromptu
{
    public static class Math
    {

        #region Public Static Methods

        public static bool IsEven(int value)
        {
            return (value % 2 == 0);
        }

        public static T Median<T>(this IEnumerable<T> source)
        {
            T[] temp = source.ToArray();    
            Array.Sort(temp);

            int count = temp.Length;
            if(count <= 0)
                throw new InvalidOperationException("Empty collection");

            if(count % 2 == 0)
            {
                // count is even, average two middle elements
                dynamic a = temp[count / 2 - 1];
                dynamic b = temp[count / 2];
                object result = ((a + b) / 2);
                return result.Cast<T>();
            }
            else
            {
                // count is odd, return the middle element
                object result = (temp[count / 2]);
                return result.Cast<T>();
            }
        }

        public static T Mean<T>(this IEnumerable<T> source)
        {
            var temp = source; 
            var count = temp.Count();
            return temp.Mean(0, count);
        }

        public static T Mean<T>(this IEnumerable<T> source, int start, int end)
        {
            T[] temp = source.ToArray(); 

            int count = temp.Length;
            if(count <= 0)
                throw new InvalidOperationException("Empty collection");
            if(count < end || start > count || start > end)
                throw new InvalidOperationException("Index out of range");

            dynamic s = 0;
            for(var i = start; i < end; i++)
                s += temp[i];

            object result = (s / (end - start));
            return result.Cast<T>();
        }

        public static T Variance<T>(this IEnumerable<T> source)
        {
            var temp = source; 
            var count = temp.Count();
            return temp.Variance(temp.Mean(), 0, count);
        }

        public static T Variance<T>(this IEnumerable<T> source, T mean, int start, int end)
        {
            T[] temp = source.ToArray(); 

            int count = temp.Length;
            if(count <= 0)
                throw new InvalidOperationException("Empty collection");
            if(count < end || start > count || start > end)
                throw new InvalidOperationException("Index out of range");

            dynamic variance = 0;
            for(var i = start; i < end; i++)
            {
                dynamic t = temp[i];
                dynamic p = (t - mean);
                variance += System.Math.Pow(p, 2);
            }

            dynamic n = end - start;
            if(start > 0)
                n -= 1;

            object result = (variance / (n));
            return result.Cast<T>();
        }

        public static double[][] Normalize(double[][] values, double min, double max)
        {
            var data = (double[][])values.Clone();
            var lists = new List<List<double>>();

            for(var i = 0; i < data[0].Length; i++)
            {                
                lists.Add(new List<double>());
                for(var ii = 0; ii < data.Length; ii++)
                {
                    var value = data[ii][i];
                    lists[i].Add(value);
                }
            }

            for(var i = 0; i < lists.Count; i++)
                lists[i] = ((double[])Normalize(lists[i].ToArray(), min, max).Clone()).ToList();

            for(var i = 0; i < data.Length; i++)
            {
                for(var ii = 0; ii < data[0].Length; ii++)
                    data[i][ii] = lists[ii][i];
            }
            return data;
        }

        public static double[] Normalize(double[] list, double min, double max)
        {
            var enumerable = list as double[] ?? list.ToArray();
            var valueMax = enumerable.Max();
            var valueMin = enumerable.Min();
            var valueRange = valueMax - valueMin;
            var scaleRange = max - min;

            var result = enumerable.Select(i => ((scaleRange * (i - valueMin)) / valueRange) + min).ToArray();
            return NaN(result, 0);
        }

        private static double[] NaN(double[] values, double set)
        {
            var result = new List<double>();
            foreach(var value in values)
            {
#pragma warning disable
                if(value != value) //NaN check
#pragma warning restore
                    result.Add(set);
                else
                    result.Add(value);
            }

            return result.ToArray();
        }

        #endregion

    }
}
