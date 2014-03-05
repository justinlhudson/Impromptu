using System;
using ServiceStack.Redis;

namespace Impromptu.Messaging.Redis
{
    public class RedisBase
    {
        private readonly RedisClient _client;

        public RedisClient RedisClient { get { return _client; } }

        public RedisBase(string host = "localhost", int port = 6379, string password = null, long db = 0) // defaults
        {
            _client = new RedisClient(host, port, password, db);                          
        }
    }
}

