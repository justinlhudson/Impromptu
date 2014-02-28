using System;
using ServiceStack.Redis;
using Impromptu.Utilities;

namespace Impromptu.Messaging.Redis.PublishSubscribe
{
    public class PublishSubscribe
    {
        public delegate void Message(string channel, object value);

        public event Message OnMessage;

        private readonly RedisClient _client;
        private readonly IRedisSubscription _subscription;

        public PublishSubscribe(string host = "localhost", int port = 6379, string password = null, long db = 0) // defaults
        {
            _client = new RedisClient(host, port, password, db);
            _subscription = _client.CreateSubscription();

            _subscription.OnMessage = (channel, message) =>
            {
                var value = Serializer.String.Deserialize<object>(message);

                var evtCopy = OnMessage;
                if(evtCopy != null)
                    evtCopy(channel, value);
            };                               
        }

        void Publish(string channel, object value)
        {
            var temp = Serializer.String.Serialize<object>(value);
            _client.PublishMessage(channel, temp);
        }

        void Subscribe(string[] channels)
        {
            _subscription.SubscribeToChannels(channels);
        }

        void UnSubscribe(string[] channels = null)
        {
            if(channels != null)
                _subscription.UnSubscribeFromChannels(channels);
            else
                _subscription.UnSubscribeFromAllChannels();
        }
    }
}

