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
        public Guid Id { get; set; }
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
            var collection = database.GetCollection<EntityData>("entityStorage");
            var filter = Builders<EntityData>.Filter.Eq("Id", id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result == null)
            {
                return default(T);
            }
            return (T)Activator.CreateInstance(typeof(T),
                new object[] { JsonConvert.DeserializeObject<K>(result.Data) });
        }

        public void Save<T>(AggregateRoot<T> aggregate, int expectedVersion = -1) where T : IAggregateEntity
        {
            var client = new MongoClient(_connectionString);
            using (var session = client.StartSession())
            {
                var database = client.GetDatabase("cqrs");
                //database.CreateCollection("entityStorage");
                var collection = database.GetCollection<EntityData>("entityStorage");
                try
                {
                    var entity = aggregate.Entity;
                    var entityData = new EntityData
                    {
                        Id = entity.Id,
                        Data = JsonConvert.SerializeObject(entity),
                        Version = entity.Version
                    };

                    var filter = Builders<EntityData>.Filter.And(Builders<EntityData>.Filter.Eq("Id", entity.Id), Builders<EntityData>.Filter.Eq("Version", expectedVersion));
                    var result = collection.ReplaceOne(
                        filter, entityData, new UpdateOptions { IsUpsert = true, });

                    if (result.MatchedCount == 0 && result.UpsertedId == null)
                    {
                        var existing = collection.Find(filter).FirstOrDefault();
                        if (existing != null && existing.Version < expectedVersion)
                        {
                            throw new LostUpdateException();
                        }
                        throw new ConcurrencyException();
                    }
                    foreach (ItemToSend @event in aggregate.GetUnsentEvents())
                    {
                        _bus.Send(@event.Data, @event.DelaySend);
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
