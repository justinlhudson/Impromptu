using System.Collections.Generic;
using System.Linq;

namespace Impromptu
{
    public static class Math
    {

        #region Public Static Methods

        public static bool IsEven(int value)
        {
            return (value % 2 == 0);
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
                if (value != value) //NaN check
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
