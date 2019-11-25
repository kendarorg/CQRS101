using MongoDB.Driver;
using SimplestPossibleThing.Lib.Projection;
using System;
using System.Collections.Generic;
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
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemDetails");
            collection.DeleteOne(toDelete => toDelete.Id == id);
        }

        public IEnumerable<InventoryItemListDto> GetAll()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemDetails");
            return collection.Find(document => true).ToList();
        }

        public InventoryItemListDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemDetails");
            return collection.Find(document => document.Id == id).FirstOrDefault();
        }

        public void Save(InventoryItemListDto InventoryItemListDto)
        {

            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<InventoryItemListDto>("InventoryItemDetails");
            collection.ReplaceOne<InventoryItemListDto>(
                        filter: dt => dt.Id == InventoryItemListDto.Id,
                        options: new UpdateOptions { IsUpsert = true },
                        replacement: InventoryItemListDto);
        }
    }
}
