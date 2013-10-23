using System.Xml.Serialization;
using System.IO;

namespace Impromptu
{
    public static class Serializer
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
}

