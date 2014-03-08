using System;
using ServiceStack.Redis;
using Impromptu.Utilities;
using System.Threading;
using System.Collections.Generic;

namespace Impromptu.Messaging.Redis
{
    public class PublishSubscribe : RedisBase, IDisposable
    {
        public delegate void Message(string channel, object value);

        public event Message OnMessage;

        private readonly List<RedisClient> _client = new List<RedisClient>();
        private readonly IRedisSubscription _subscription;
        private Thread _blocking;

        public PublishSubscribe(string host = "localhost", int port = 6379, string password = null, long db = 0, bool publishToSelf = true) : base(host, port, password, db)
        {
            _client.Add(RedisClient);
            _subscription = _client[0].CreateSubscription();

            if(publishToSelf)
                _client.Add((new RedisBase(host, port, password, db)).RedisClient);

            _subscription.OnSubscribe = channel =>
            {
                ;
            };
            _subscription.OnUnSubscribe = channel =>
            {
                ;
            };

            _subscription.OnMessage = (channel, message) =>
            {
                var value = Serializer.String.Deserialize<object>(message);

                var evtCopy = OnMessage;
                if(evtCopy != null)
                    evtCopy(channel, value);
            };                               
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Close();
        }

        #endregion

        public void Publish(string channel, object value)
        {
            var temp = Serializer.String.Serialize<object>(value);
            if(_client.Count > 1)
                _client[1].PublishMessage(channel, temp);
            else
                _client[0].PublishMessage(channel, temp);
        }

        public void Subscribe(string[] channels)
        {
            UnSubscribe();

            if(_blocking != null && _blocking.IsAlive)
                _blocking.Abort();

            _blocking = new Thread(new ThreadStart(() => _subscription.SubscribeToChannels(channels)));
            _blocking.Start();                
        }

        public void UnSubscribe(string[] channels = null)
        {
            if(channels != null)
                _subscription.UnSubscribeFromChannels(channels);
            else
                _subscription.UnSubscribeFromAllChannels();
        }

        public void Close()
        {
            if(_blocking != null && _blocking.IsAlive)
                _blocking.Abort();
        }
    }
}

