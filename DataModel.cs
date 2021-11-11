using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json.Linq;

namespace si_net_project_consumer
{
    public class DataModel
    {
        [BsonElement("Value")] 
        public string Value { get; }
        
        [BsonElement("Date")] 
        public DateTime DateTime { get; }
        
        [BsonElement("Hive")] 
        public int HiveId { get; }

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))] 
        public ObjectId? Id { get; set; }

        public DataModel(string message)
        {
            dynamic data = JObject.Parse(message);
            string idFromQueue = data.id;
            double timestampFromqueue = data.timestamp;
            
            HiveId = idFromQueue[^1];
            Value = data.value;
            DateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime = DateTime.AddMilliseconds(timestampFromqueue).ToLocalTime();

        }
    }
}