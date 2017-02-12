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

		[StringLength(8)]
		public string List { get; set; }

		public object Value { get; set; }

	}
}
