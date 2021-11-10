using System;
using RabbitMQ.Client;

namespace si_net_project_consumer
{
    class Program
    {
        private const string ConnectionString = "mongodb://root:example@mongodb:27017/";
        private const string DatabaseName = "SI_17555";
        private const string Host = "rabbitmq";
        private const string TemperatureCollection = "temperature";
        
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = Host };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: TemperatureCollection,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    var temperatureConsumer = new Consumer(channel, ConnectionString, DatabaseName, TemperatureCollection);
                    channel.BasicConsume(TemperatureCollection, false, temperatureConsumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
