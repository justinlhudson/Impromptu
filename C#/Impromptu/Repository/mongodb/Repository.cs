/// <Fork>
/// http://mongorepository.codeplex.com/
/// </Fork>
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Wrappers;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Linq.Utils;
using Inflector;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Impromptu.Repository.Mongo
{
  public class Repository
  {
    public MongoClient Client { get; private set; }

    public IMongoDatabase Database { get; private set; }

    public Repository(string database, string connectionString = "mongodb://localhost", bool clear = false)
    {
      try
      {
        connectionString += "/" + database;

        Client = new MongoClient(connectionString);
        
        if (Client.Settings.MaxConnectionPoolSize > 5)
          Client.Settings.MaxConnectionPoolSize = 5;
        
        var mongoUrl = new MongoUrl(connectionString);

        //if (clear)
        //  Server.DropDatabase(mongoUrl.DatabaseName);

        Database = Client.GetDatabase(mongoUrl.DatabaseName);

      } catch (Exception)
      {
        throw;
      }
    }

    public string MemoryCache<T>(bool data, bool index)  where T : IEntity
    {
      var commands = new Dictionary<string,object>();
      commands.Add("touch", GetCollectionName<T>());
      commands.Add("data", data.ToString().ToLower());
      commands.Add("index", index.ToString().ToLower());
      return Run(commands);
    }

    public string Run(Dictionary<string, object> commands)
    {     
      var result = string.Empty;
      try
      {
        var textSearchCommand = new CommandDocument();
        textSearchCommand.AddRange(commands);

        var commandResult = Database.RunCommandAsync<dynamic>(textSearchCommand).Result;
        result = commandResult;
      } catch (Exception ex)
      {
        result = ex.ToString();
      }
      return result;
    }

    public IEnumerable<T> Find<T>(string code, string sortBy = "", bool sortDescending = false, int limit = int.MaxValue) where T : IEntity
    {
      var builderSort = Builders<T>.Sort;
      var sortedBy = builderSort.Ascending(sortBy);
      if (sortDescending)
        sortedBy = builderSort.Descending(sortBy);

      FilterDefinition<T> filter = code;

      var cursor = GetCollection<T>().Find<T>(filter).Sort(sortedBy).Limit(limit);
      // HACK: 2.1.? expect release:
      //return GetCollection<T>().AsQueryable<T>();
      var temp = cursor.ToListAsync().Result.AsEnumerable();
      return temp;
    }

    public void Delete<T>(T entry) where T : IEntity
    {
      var builderFilter = Builders<T>.Filter;
      var filter = builderFilter.Eq(e => e.id, entry.id);
      GetCollection<T>().DeleteOneAsync(filter);
    }

    public void SaveOrUpdate<T>(T entry) where T : IEntity
    {
      // insert or update
      // update: not working correctly so removing then save
      Delete(entry);
      GetCollection<T>().InsertOneAsync(entry); 
      return;
    }

    public IMongoCollection<T> GetCollection<T>() where T : IEntity
    {
      return Database.GetCollection<T>(GetCollectionName<T>());
    }

    public IQueryable<T> AsQueryable<T>(Expression<Func<T,bool>> rule) where T : IEntity
    {
      // HACK: 2.1.? expect release:
      //return GetCollection<T>().AsQueryable<T>();
      var builderFilter = Builders<T>.Filter;

      var filter = builderFilter.Where(rule); // all = builderFilter.Where(rule); // all
      var list = GetCollection<T>().Find(filter).ToListAsync();  
      
      return list.Result.AsQueryable();
    }
    /*
    public IQueryable<T> AsQueryable<T>() where T : IEntity
    {
      // HACK: 2.1.? expect release:
      //return GetCollection<T>().AsQueryable<T>();
      var builderFilter = Builders<T>.Filter;
      var filter = builderFilter.Where.NE(e => e.id, new BsonObjectId(new ObjectId())); // all?
      var list = GetCollection<T>().Find(filter).ToListAsync();  
      
      return list.Result.AsQueryable();
    }
*/
    private string GetCollectionName<T>() where T : IEntity
    {
      string collectionName;
      if (typeof(T).BaseType.Equals(typeof(object)))
        collectionName = GetCollectionNameFromInterface<T>();
      else
        collectionName = GetCollectionNameFromType(typeof(T));

      if (string.IsNullOrEmpty(collectionName))
        throw new ArgumentException("Collection name cannot be empty for this entity");
      return collectionName.Pluralize();
    }

    private static string GetCollectionNameFromInterface<T>()
    {
      string collectionname;

      // Check to see if the object (inherited from Entity) has a CollectionName attribute
      var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
      if (att != null)
      {
        // It does! Return the value specified by the CollectionName attribute
        collectionname = ((CollectionName)att).name;
      }
      else
      {
        collectionname = typeof(T).Name;
      }

      return collectionname.Pluralize();
    }

    private static string GetCollectionNameFromType(Type entitytype)
    {
      string collectionname;

      // Check to see if the object (inherited from Entity) has a CollectionName attribute
      var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionName));
      if (att != null)
      {
        // It does! Return the value specified by the CollectionName attribute
        collectionname = ((CollectionName)att).name;
      }
      else
      {
        // No attribute found, get the basetype
        while (!entitytype.BaseType.Equals(typeof(Entity)))
          entitytype = entitytype.BaseType;
        collectionname = entitytype.Name;
      }

      return collectionname.Pluralize();
    }
  }
}

