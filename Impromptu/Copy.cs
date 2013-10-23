using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Impromptu
{
    public static class Copy
    {
        /// <summary>
        ///   Deep copy in separeate memory space
        /// </summary>
        /// <returns></returns>
        public static T Deep <T>(T obj)
        {
            var ms = new MemoryStream(); // implicit type
            var bf = new BinaryFormatter(); // implicit type

            bf.Serialize(ms, obj);
            ms.Position = 0;

            var copy = (T)bf.Deserialize(ms);
            ms.Close();
            
            return copy;
        }
    }
}
