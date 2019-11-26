using Infrastructure.Lib.Cqrs;
using Infrastructure.Lib.ServiceBus;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mongo.Cqrs
{
    public class EntityData
    {
        public Guid _id { get; set; }
        public int Version { get; set; }
        public string Data { get; set; }
    }
    public class MongoEntityStorage : IEntityStorage
    {
        private readonly string _connectionString;
        private readonly IBus _bus;

        public MongoEntityStorage(IBus bus, string connectionString)
        {
            _connectionString = connectionString;
            _bus = bus;
        }

        public T GetById<T, K>(Guid id) where T : IAggregateRoot
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("entityStorage");
            var collection = database.GetCollection<BsonDocument>("entityStorage");
            var filter = Builders<BsonDocument>.Filter.Eq("_id",new ObjectId( id.ToString().Replace("-","")));
            var result = collection.Find(filter).FirstOrDefault();
            if (result == null)
            {
                return default(T);
            }
            return (T)Activator.CreateInstance(typeof(T),
                new object[] { JsonConvert.DeserializeObject<K>(result["Data"].AsString) });
        }

        public void Save<T>(AggregateRoot<T> aggregate, int expectedVersion = -1) where T : IAggregateEntity
        {
            var client = new MongoClient(_connectionString);
            using (var session = client.StartSession())
            {
                var database = client.GetDatabase("cqrs");
                //database.CreateCollection("entityStorage");
                var collection = database.GetCollection<BsonDocument>("entityStorage");
                try
                {
                    var entity = aggregate.Entity;
                    var data = new BsonDocument();
                    data["_id"] = new ObjectId(entity.Id.ToString().Replace("-",""));
                    data["Version"] = entity.Version;
                    data["Data"] = JsonConvert.SerializeObject(entity);

                    var filter = Builders<BsonDocument>.Filter.Eq("_id", data["_id"]);
                    var result = collection.ReplaceOne(
                        filter, data, new UpdateOptions { IsUpsert = true });

                    if (result.MatchedCount == 0 && result.UpsertedId == null)
                    {
                        var existing = collection.Find(filter).FirstOrDefault();
                        if(existing!=null && existing["Version"].AsInt32 < expectedVersion)
                        {
                            throw new LostUpdateException();
                        }
                        throw new ConcurrencyException();
                    }
                    foreach (var @event in aggregate.GetUnsentEvents())
                    {
                        _bus.Send(@event);
                    }
                }
                catch (Exception ex)
                {
                    session.AbortTransactionAsync();
                    throw ex;
                }
            }
        }
    }
}
