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



        private static InventoryItemListDto ToDto(BsonDocument bson)
        {
            return new InventoryItemListDto(bson["_id"].AsGuid, bson["Name"].AsString);
        }

        private static BsonDocument FromDto(InventoryItemListDto bson)
        {
            var doc = new BsonDocument();
            doc["_id"] = bson.Id.ToString().Replace("-", "");
            doc["Name"] = bson.Name;
            return doc;
        }
        public MongoInventoryItemListRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Delete(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemList");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id.ToString().Replace("-", "")));
            collection.DeleteOne(filter);
        }

        public IEnumerable<InventoryItemListDto> GetAll()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemList");
            return collection.AsQueryable().Select(bd => ToDto(bd));
        }

        public InventoryItemListDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemList");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id.ToString().Replace("-", "")));
            var bson = collection.Find(filter).FirstOrDefault();
            if (bson == null)
            {
                return null;
            }
            return ToDto(bson);
        }

        public void Save(InventoryItemListDto inventoryItemListDto)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemList");

            var objectID = new ObjectId(inventoryItemListDto.Id.ToString().Replace("-", ""));

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectID);
            var result = collection.ReplaceOne(
                filter, FromDto(inventoryItemListDto), new UpdateOptions { IsUpsert = true });
        }
    }
}
