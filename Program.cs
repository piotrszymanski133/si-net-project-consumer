using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace si_net_project_consumer
{
    class Program
    {
        private const string ConnectionString = "mongodb://root:student@localhost:27017/";
        private const string DatabaseName = "SI_17555";
        private const string Host = "rabbitmq";
        private const string TemperatureCollection = "temperature";
        private const string HumidityCollection = "humidity";
        private const string WindCollection = "wind";
        private const string PressureCollection = "pressure";
        
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
                    
                    channel.QueueDeclare(queue: HumidityCollection,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    channel.QueueDeclare(queue: WindCollection,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    channel.QueueDeclare(queue: PressureCollection,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    var temperatureConsumer = new Consumer(channel, ConnectionString, DatabaseName, TemperatureCollection);
                    var humidityConsumer = new Consumer(channel, ConnectionString, DatabaseName, HumidityCollection);
                    var windConsumer = new Consumer(channel, ConnectionString, DatabaseName, WindCollection);
                    var pressureConsumer = new Consumer(channel, ConnectionString, DatabaseName, PressureCollection);
                    
                    channel.BasicConsume(TemperatureCollection, false, temperatureConsumer);
                    channel.BasicConsume(HumidityCollection, false, humidityConsumer);
                    channel.BasicConsume(WindCollection, false, windConsumer);
                    channel.BasicConsume(PressureCollection, false, pressureConsumer);
                    while (true)
                    {
                    }
                }
            }
        }
    }
}
