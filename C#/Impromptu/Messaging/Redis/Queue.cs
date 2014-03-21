using System;
using ServiceStack.Redis;
using Impromptu.Utilities;

namespace Impromptu.Messaging.Redis
{
  public class Queue : RedisBase, IQueue
  {
    public Queue(string host = "localhost", int port = 6379, string password = null, long db = 0) : base(host, port, password, db) // defaults
    {                          
    }

    public void Flush(string list)
    {
      while(Pop<object>(list) != null)
        ;
    }

    public long Length(string list)
    {
      var temp = RedisClient.LLen(list);
      return  temp;
    }

    public void Push<T>(string list, T value)
    {
      var temp = Serializer.Bytes.Serialize<T>(value);
      RedisClient.RPush(list, temp);
    }

    public T Pop<T>(string list)
    {
      var temp = RedisClient.RPop(list);
      if(temp == null || temp.Length <= 0)
        return default(T);
      return Serializer.Bytes.Deserialize<T>(temp);
    }
  }
}

