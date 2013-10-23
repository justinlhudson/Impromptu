#region

using System;
using System.Collections.Generic;

#endregion
namespace Impromptu
{
    /// <summary>
    ///   Conversion class.
    /// </summary>
    public class Conversion <T>
    {

        #region Fields

        private char[] m_delimiter = { ',', '\n' };
        private char[] m_remove = { ' ', '\t', '\b', '\r' };

        #endregion

        #region Properties

        public char[] Delimiter {
            get { return m_delimiter; }
            set { m_delimiter = value; }
        }

        public char[] Remove {
            get { return m_remove; }
            set { m_remove = value; }
        }

        #endregion

        #region Public Methods

        public byte[] ToByteArray(T[] value)
        {
            var data = new List<byte>();
            for(var i = 0; i < data.Count; i++)
                data.Add(Convert.ToByte(value[i]));
            return data.ToArray();
        }

        public double[] ToDoubleArray(T[] value)
        {
            double[] result;
            result = new double[value.Length];
            for(var i = 0; i < value.Length; i++)
                result[i] = Convert.ToDouble(value[i]);
            return result;
        }

        public double[] ToDoubleArray(string value)
        {
            double[] result;
            var data = value;

            data.Trim();
            data = data.Trim(m_remove);
            var split = data.Split(m_delimiter);
            result = new double[split.Length];

            for(var i = 0; i < split.Length; i++)
                result[i] = Convert.ToDouble(split[i]);

            return result;
        }

        public string[] ToStringArray(T[] value)
        {
            var result = new string[value.Length];
            for(var i = 0; i < result.Length; i++)
                result[i] = Convert.ToString(value[i]);
            return result;
        }

        public string ToString(T[] value)
        {
            var result = string.Empty;
            var data = value;

            for(var i = 0; i < data.Length; i++)
                result += Convert.ToString(data[i]) + m_delimiter[0];

            return result;
        }

        public int[] ToIntArray(string value)
        {
            var data = value;
            data = data.Trim(m_remove);

            var split = data.Split(m_delimiter);
            var result = new int[split.Length];

            for(var i = 0; i < split.Length; i++)
                result[i] = Convert.ToInt32(split[i]);

            return result;
        }

        public byte[] ToByteArray(double[] value, int bytesPerDouble = 8)
        {
            var result = new byte[value.Length * bytesPerDouble];

            for(var i = 0; i < value.Length; i++)
            {
                var temp = BitConverter.GetBytes(value[i]);
                for(var ii = 0; ii < temp.Length; ii++)
                    result[(i * bytesPerDouble) + ii] = temp[ii];
            }

            return result;
        }

        public double ToDouble(byte[] value)
        {
            double result;
            result = BitConverter.ToDouble(value, 0);
            return result;
        }

        #endregion

    }
}
