using System;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Impromptu.Utilities;

namespace Impromptu.Messaging.MongoDB
{
	public class Queue : MongoBase, IQueue
	{

		public Queue(string connection = "localhost:27017", long db = 0)
		  : base(connection, db) // defaults
		{
			base.Dispose();
		}

		public void Flush(string key)
		{
			while (Pop<object>(key) != null)
				;
		}

		public long Length(string key)
		{
			return MessageQueue.AsQueryable().Where(item => item.Key == key).CountAsync().Result;
		}

		public void Push<T>(string key, T value)
		{
			MessageQueue.InsertOne(new DataType { Key = key, Value = (object)value });
		}

		public T Pop<T>(string key)
		{
			var docs = MessageQueue.AsQueryable().Where(item => item.Key == key).OrderBy(item => item.Id).Take(1).ToList();

			DataType doc = null;
			if (docs.Count > 0)
				doc = docs[0];

			if (doc != null)
			{
				var builder = Builders<DataType>.Filter;
				var filter = builder.Eq("_id", doc.Id);
				MessageQueue.DeleteOne(filter);

				return (T)doc.Value;
			}

			return default(T);
		}
	}
}
