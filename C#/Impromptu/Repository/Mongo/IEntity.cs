using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Impromptu.Repository.Mongo
{
  /// <summary>
  /// Entity interface.
  /// </summary>
  public interface IEntity
  {
    /// <summary>
    /// Gets or sets the Id of the Entity.
    /// </summary>
    /// <value>Id of the Entity.</value>
    [BsonId]
    BsonObjectId id { get; set; }
  }
}