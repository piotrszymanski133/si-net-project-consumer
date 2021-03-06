using System;
using System.Text;
using MongoDB.Driver;
using RabbitMQ.Client;


namespace si_net_project_consumer
{
    public class Consumer : DefaultBasicConsumer
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private string _collection;
        public Consumer(IModel model, string connectionString, string databaseName, string collection) : base(model)
        {
            try
            {
                _mongoClient = new MongoClient(connectionString);
                _mongoDatabase = _mongoClient.GetDatabase(databaseName);
                _collection = collection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            try
            {
                var message = Encoding.UTF8.GetString(body.ToArray());
                DataModel model = new DataModel(message);

                var data = _mongoDatabase.GetCollection<DataModel>(_collection);
                data.InsertOne(model);
                var readAll = data.Find(temperature => true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

}