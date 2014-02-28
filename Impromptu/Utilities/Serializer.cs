using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Impromptu.Utilities
{
    public static class Serializer
    {
        public static class String
        {
            public static string Serialize<T>(T value)
            {
                var result = string.Empty;
                var serializer = new XmlSerializer(typeof(T));
                using(var stream = new StringWriter())
                {
                    serializer.Serialize(stream, value);
                    result = stream.ToString();
                }
                return result;
            }

            public static T Deserialize<T>(string value)
            {
                var result = default(T);
                var serializer = new XmlSerializer(typeof(T));
                using(var stream = new StringReader(value))
                    result = (T)serializer.Deserialize(stream);
                return result;
            }
        }

        public static class Bytes
        {
            public static byte[] Serialize<T>(T value)
            {
                if(value == null)
                    return null;
                var bf = new BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, value);
                return ms.ToArray();
            }

            public static T Deserialize<T>(byte[] value)
            {
                var memStream = new MemoryStream();
                var binForm = new BinaryFormatter();
                memStream.Write(value, 0, value.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var result = (T)binForm.Deserialize(memStream);
                return result;
            }
        }
    }
}

