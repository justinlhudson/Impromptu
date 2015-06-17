using System;
using System.Runtime.Serialization;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Impromptu.Repository.Mongo
{
  /// <summary>
  /// Abstract Entity for all the BusinessEntities.
  /// </summary>
  [DataContract]
  [Serializable]
  [BsonIgnoreExtraElements(Inherited = true)]
  public abstract class Entity : IEntity
  {
    /// <summary>
    /// Gets or sets the id for this object (the primary record for an entity).
    /// </summary>
    /// <value>The id for this object (the primary record for an entity).</value>
    [DataMember]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual BsonObjectId id { get; set; }
  }
}