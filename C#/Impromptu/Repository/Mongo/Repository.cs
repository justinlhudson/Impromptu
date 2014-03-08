/// <Fork>
/// http://mongorepository.codeplex.com/
/// </Fork>

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Options;
using Inflector;

namespace Impromptu.Repository.Mongo
{
    public class Repository
    {
        public MongoClient Client { get; private set; }

        public MongoServer Server { get; private set; }

        public MongoDatabase Database { get; private set; }

        public Repository(string database, string connectionString = "mongodb://localhost", bool clear = false)
        {
            try
            {
                connectionString += "/" + database;

                Client = new MongoClient(connectionString);
                Server = Client.GetServer();		

                var mongoUrl = new MongoUrl(connectionString);

                if(clear)
                    Server.DropDatabase(mongoUrl.DatabaseName);

                Database = Server.GetDatabase(mongoUrl.DatabaseName);	   
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IEnumerable<T> Find<T>(string code, int limit = int.MaxValue) where T : IEntity
        {
            var document = BsonSerializer.Deserialize<BsonDocument>(code);
            var queryDoc = new QueryDocument(document);
            var cursor = GetCollection<T>().FindAs<T>(queryDoc).SetLimit(limit);
            return cursor as IEnumerable<T>;
        }

        public void Delete<T>(T entry) where T : IEntity
        {
            var query = Query<Entity>.EQ(e => e.id, entry.id);
            GetCollection<T>().Remove(query);
        }

        public void SaveOrUpdate<T>(T entry) where T : IEntity
        {
            GetCollection<T>().Save(entry); // insert or update
        }

        public MongoCollection<T> GetCollection<T>() where T : IEntity
        {
            return Database.GetCollection<T>(GetCollectionName<T>());
        }

        public IQueryable<T> AsQueryable<T>() where T : IEntity
        {
            return GetCollection<T>().AsQueryable<T>();
        }

        private string GetCollectionName<T>() where T : IEntity
        {
            string collectionName;
            if(typeof(T).BaseType.Equals(typeof(object)))
                collectionName = GetCollectionNameFromInterface<T>();
            else
                collectionName = GetCollectionNameFromType(typeof(T));

            if(string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("Collection name cannot be empty for this entity");
            return collectionName.Pluralize();
        }

        private static string GetCollectionNameFromInterface<T>()
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));
            if(att != null)
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
            if(att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionName)att).name;
            }
            else
            {
                // No attribute found, get the basetype
                while(!entitytype.BaseType.Equals(typeof(Entity)))
                    entitytype = entitytype.BaseType;
                collectionname = entitytype.Name;
            }

            return collectionname.Pluralize();
        }
    }
}

