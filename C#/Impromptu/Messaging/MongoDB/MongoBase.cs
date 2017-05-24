using System;
using MongoDB.Driver;

namespace Impromptu.Messaging.MongoDB
{
	public class MongoBase : IDisposable
	{

		private readonly IMongoDatabase _db;
		private string _collection = "_MessageQueue";

		public MongoBase(string connection, long db = 0) // defaults
		{
			_collection += "_" + db.ToString();

			var connectionString = connection;
			var url = new MongoUrl(connectionString);
			var settings = new MongoClientSettings()
			{
				Server = url.Server,
				WaitQueueTimeout = new TimeSpan(0, 5, 0)
			};
			var client = new MongoClient(settings);
			_db = client.GetDatabase(url.DatabaseName);
		}

		public IMongoCollection<DataType> MessageQueue { get { return _db.GetCollection<DataType>(_collection); } }

		public void Dispose()
		{
			//_db.DropCollection(_collection);
		}
	}
}
