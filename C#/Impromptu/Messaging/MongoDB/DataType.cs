using System;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace Impromptu
{
	public class DataType
	{
		[BsonId]
		public ObjectId Id { get; set; }

		[StringLength(64)]
		public string Key { get; set; }

		public object Value { get; set; }

	}
}
