/*
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
            //password
            if(host.Contains(","))
            {
                var split = host.Split(new[]{ ',' }, 2);
                host = split[0];
                password = split[1];
            }

            // port
            if(host.Contains(":"))
            {
                // should throw if messed up so no checking done here
                var split = host.Split(new[]{ ':' });
                host = split[0];
                port = (int)Convert.ToInt64(split[1]);
            }
                
            _client = new RedisClient(host, port, password, db);                          
        }
    }
}

*/