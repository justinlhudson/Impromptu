/*
using System;
using ServiceStack.Redis;
using Impromptu.Utilities;
using ServiceStack.Redis.Generic;

namespace Impromptu.Messaging.Redis
{
    public class Storage : RedisBase
    {
        public Storage(string host = "localhost", int port = 6379, string password = null, long db = 0) : base(host, port, password, db)
        {
        }

        public IRedisList<T> List<T>(string name)
        {        
            var client = RedisClient.As<T>();
            return client.Lists[name];
        }

        public IRedisHash<T1, T2> Hash<T1,T2>(string name)
        {
            var client = RedisClient.As<T2>();
            return client.GetHash<T1>(name);             
        }
    }
}

*/