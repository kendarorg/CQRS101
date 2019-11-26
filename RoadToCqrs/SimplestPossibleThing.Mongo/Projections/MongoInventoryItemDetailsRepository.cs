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
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemDetails");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id.ToString().Replace("-", "")));
            collection.DeleteOne(filter);
        }

        public InventoryItemDetailsDto GetById(Guid id)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemDetails");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id.ToString().Replace("-", "")));
            var bson = collection.Find(filter).FirstOrDefault();
            if (bson == null)
            {
                return null;
            }
            return ToDto(bson);
        }

        private static InventoryItemDetailsDto ToDto(BsonDocument bson)
        {
            return new InventoryItemDetailsDto(bson["_id"].AsGuid, bson["Name"].AsString, bson["CurrentCount"].ToInt32(), bson["Version"].ToInt32());
        }

        private static BsonDocument FromDto(InventoryItemDetailsDto bson)
        {
            var doc = new BsonDocument();
            doc["_id"] = bson.Id.ToString().Replace("-", "");
            doc["Name"] = bson.Name;
            doc["CurrentCount"] = bson.CurrentCount;
            doc["Version"] = bson.Version;
            return doc;
        }

        public void Save(InventoryItemDetailsDto inventoryItemDetailsDto)
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("cqrs");
            //database.CreateCollection("InventoryItemDetails");
            var collection = database.GetCollection<BsonDocument>("InventoryItemDetails");
            
            var objectID = new ObjectId(inventoryItemDetailsDto.Id.ToString().Replace("-", ""));

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectID);
            var result = collection.ReplaceOne(
                filter, FromDto(inventoryItemDetailsDto), new UpdateOptions { IsUpsert = true });
        }
    }
}
