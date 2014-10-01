using System;
using StackExchange.Redis;

namespace Impromptu.Messaging.Redis
{
  public class RedisBase
  {
    private readonly ConnectionMultiplexer _client;

    public ConnectionMultiplexer RedisClient { get { return _client; } }

    public RedisBase(string host = "localhost", int port = 6379, string password = null, long db = 0) // defaults
    {
      //password
      if (host.Contains(","))
      {
        var split = host.Split(new[]{ ',' }, 2);
        host = split[0];
        password = split[1];
      }

      // port
      if (host.Contains(":"))
      {
        // should throw if messed up so no checking done here
        var split = host.Split(new[]{ ':' });
        host = split[0];
        port = (int)Convert.ToInt64(split[1]);
      }
                
      _client = ConnectionMultiplexer.Connect(host + ":" + port);                        
    }
  }
}
