using System;
using StackExchange.Redis;
using Impromptu.Utilities;

namespace Impromptu.Messaging.Redis
{
	public class Queue : RedisBase, IQueue
	{
		private IDatabase _database;

		public Queue(string host = "localhost", int port = 6379, string password = null, long db = 0)
		  : base(host, port, password, db) // defaults
		{
			_database = RedisClient.GetDatabase((int)Convert.ToInt64(db));
		}

		public void Flush(string key)
		{
			while (Pop<object>(key) != null)
				;
		}

		public long Length(string key)
		{
			var temp = _database.ListLength(key);
			return temp;
		}

		public void Push<T>(string key, T value)
		{
			var temp = Serializer.Bytes.Serialize<T>(value);
			_database.ListRightPush(key, temp);
		}

		public T Pop<T>(string key)
		{
			var temp = _database.ListRightPop(key);
			if (!temp.HasValue)
				return default(T);
			return Serializer.Bytes.Deserialize<T>(temp);
		}
	}
}
