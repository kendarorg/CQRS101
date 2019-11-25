using MongoDB.Driver;
using SimplestPossibleThing.Lib.Projection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mongo.Projections
{
    public class MongoInventoryItemDetailsRepository : IInventoryItemDetailsRepository
    {
        private readonly string _connectionString;

        public MongoInventoryItemDetailsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Delete(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");
            collection.DeleteOne(toDelete => toDelete.Id == id);
        }

        public InventoryItemDetailsDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");
            return collection.Find(document => document.Id == id).FirstOrDefault();
        }

        public void Save(InventoryItemDetailsDto inventoryItemDetailsDto)
        {

            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");
            collection.ReplaceOne<InventoryItemDetailsDto>(
                        filter: dt => dt.Id == inventoryItemDetailsDto.Id,
                        options: new UpdateOptions { IsUpsert = true },
                        replacement: inventoryItemDetailsDto);
        }
    }
}
