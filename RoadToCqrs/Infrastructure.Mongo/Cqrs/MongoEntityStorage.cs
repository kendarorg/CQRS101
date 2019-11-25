using Infrastructure.Lib.Cqrs;
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
            database.CreateCollection("entityStorage");
            var collection = database.GetCollection<EntityData>("entityStorage");
            var result = collection.Find(document => document.Id == id).FirstOrDefault();
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
                database.CreateCollection("entityStorage");
                var collection = database.GetCollection<EntityData>("entityStorage");
                try
                {
                    var entity = aggregate.Entity;
                    var data = new EntityData
                    {
                        Id = entity.Id,
                        Version = entity.Version,
                        Data = JsonConvert.SerializeObject(entity)
                    };

                    var result = collection.ReplaceOne<EntityData>(
                        filter: dt => dt.Id == data.Id,
                        options: new UpdateOptions { IsUpsert = true },
                        replacement: data);

                    if (result.MatchedCount == 0 || result.UpsertedId == null)
                    {
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
