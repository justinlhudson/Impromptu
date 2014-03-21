using System;

namespace Impromptu.Messaging
{
  public interface IQueue
  {
    void Flush(string list);

    long Length(string list);

    void Push<T>(string list, T value);

    T Pop<T>(string list);
  }
}

