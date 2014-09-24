using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Impromptu.Compare
{
  public class IEnumerableComparer<T> : IEqualityComparer<IEnumerable<T>>
  {
    #region IEqualityComparer implementation

    public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
    {
      if (x == null && y == null)
        return true;

      var flag = x.OrderBy(i => i).SequenceEqual(y.OrderBy(i => i)); 
      return flag;
    }

    public int GetHashCode(IEnumerable<T> obj)
    {
      return obj.GetHashCode();
    }

    #endregion


  }

  public class Helpers
  {
    public static bool FileEquals(string filePath1, string filePath2)
    {
      // Check the file size and CRC equality here.. if they are equal...    
      using (var file1 = new FileStream(filePath1, FileMode.Open))
        using (var file2 = new FileStream(filePath2, FileMode.Open))
          return StreamEquals(file1, file2);
    }

    public static bool StreamEquals(Stream stream1, Stream stream2)
    {
      const int bufferSize = 2048;
      var buffer1 = new byte[bufferSize]; // buffer size
      var buffer2 = new byte[bufferSize];
      while (true)
      {
        var count1 = stream1.Read(buffer1, 0, bufferSize);
        var count2 = stream2.Read(buffer2, 0, bufferSize);

        if (count1 != count2)
          return false;

        if (count1 == 0)
          return true;

        if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
          return false;
      }
    }
  }
}
