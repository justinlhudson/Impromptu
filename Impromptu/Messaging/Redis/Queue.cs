using System;
using ServiceStack.Redis;
using Impromptu.Utilities;

namespace Impromptu.Messaging.Redis
{
    public class Queue
    {
        private readonly RedisClient _client;

        public Queue(string host = "localhost", int port = 6379, string password = null, long db = 0) // defaults
        {
            _client = new RedisClient(host, port, password, db);                          
        }

        public void Flush(string list)
        {
            while(Pop<object>(list) != null)
                ;
        }

        public long Length(string list)
        {
            var temp = _client.LLen(list);
            return  temp;
        }

        public long Push<T>(string list, T value)
        {
            var temp = Serializer.Bytes.Serialize<T>(value);
            return _client.RPush(list, temp);
        }

        public T Pop<T>(string list)
        {
            var temp = _client.RPop(list);
            if(temp == null || temp.Length <= 0)
                return default(T);
            return Serializer.Bytes.Deserialize<T>(temp);
        }
    }
}

