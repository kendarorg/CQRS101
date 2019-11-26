using MongoDB.Bson;
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
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");
            var filter = Builders<InventoryItemDetailsDto>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }

        public InventoryItemDetailsDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");
            var filter = Builders<InventoryItemDetailsDto>.Filter.Eq("Id", id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public void Save(InventoryItemDetailsDto inventoryItemDetailsDto)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemDetailsDto>("InventoryItemDetails");

            var filter = Builders<InventoryItemDetailsDto>.Filter.Eq("_id", inventoryItemDetailsDto.Id);
            var result = collection.ReplaceOne(
                filter, inventoryItemDetailsDto, new UpdateOptions { IsUpsert = true });
        }
    }
}
