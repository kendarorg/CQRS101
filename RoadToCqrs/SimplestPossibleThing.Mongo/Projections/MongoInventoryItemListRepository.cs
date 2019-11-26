using MongoDB.Bson;
using MongoDB.Driver;
using SimplestPossibleThing.Lib.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Mongo.Projections
{
    public class MongoInventoryItemListRepository : IInventoryItemListRepository
    {
        private readonly string _connectionString;

        public MongoInventoryItemListRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Delete(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemList");
            var filter = Builders<InventoryItemListDto>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }

        public IEnumerable<InventoryItemListDto> GetAll()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemList");
            return collection.AsQueryable();
        }

        public InventoryItemListDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemList");
            var filter = Builders<InventoryItemListDto>.Filter.Eq("_id", id);
            var result = collection.Find(filter).FirstOrDefault();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public void Save(InventoryItemListDto inventoryItemListDto)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemList");


            var filter = Builders<InventoryItemListDto>.Filter.Eq("Id", inventoryItemListDto.Id);
            var result = collection.ReplaceOne(
                filter, inventoryItemListDto, new UpdateOptions { IsUpsert = true });
        }
    }
}
