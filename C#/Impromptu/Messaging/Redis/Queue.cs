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

		public void Flush(string list)
		{
			while (Pop<object>(list) != null)
				;
		}

		public long Length(string list)
		{
			var temp = _database.ListLength(list);
			return temp;
		}

		public void Push<T>(string list, T value)
		{
			var temp = Serializer.Bytes.Serialize<T>(value);
			_database.ListRightPush(list, temp);
		}

		public T Pop<T>(string list)
		{
			var temp = _database.ListRightPop(list);
			if (!temp.HasValue)
				return default(T);
			return Serializer.Bytes.Deserialize<T>(temp);
		}
	}
}
