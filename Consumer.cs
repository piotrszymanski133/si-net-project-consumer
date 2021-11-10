using System;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using RabbitMQ.Client;


namespace si_net_project_consumer
{
    public class Consumer : DefaultBasicConsumer
    {
        private int _id = 0;
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private string _collection;
        public Consumer(IModel model, string connectionString, string databaseName, string collection) : base(model)
        {
            _mongoClient = new MongoClient(connectionString);
            _mongoDatabase = _mongoClient.GetDatabase(databaseName);
            _collection = collection;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());
            TestModel model = new TestModel(message, _id);
            _id++;
            
            var data = _mongoDatabase.GetCollection<TestModel>(_collection);
            data.InsertOne(model);
            var readAll = data.Find( temperature => true);
        }
    }


    public class TestModel
    {
        [BsonElement("Name")] 
        public string Temperature { get; }
        
        [BsonId] [BsonRepresentation(BsonType.Int32)]
        public int id;

        public TestModel(string temperature, int id)
        {
            Temperature = temperature;
            this.id = id;
        }
    }
}