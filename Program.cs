using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace si_net_project_consumer
{
    class Program
    {
        private const string ConnectionString = "mongodb://root:student@admin-mongodb_mongo:27017/";
        private const string DatabaseName = "SI_17555";
        private const string Host = "rabbitmq-SI-175557";
        private const string TemperatureCollection = "temperature";
        private const string HumidityCollection = "humidity";
        private const string WindCollection = "wind";
        private const string PressureCollection = "pressure";

        static void Main(string[] args)
        {
            var remainingMessages = 20;
            var factory = new ConnectionFactory() {HostName = Host};
            try
            {
                Console.WriteLine("TRYING TO CONNECT");
                using (var connection = factory.CreateConnection())
                {
                    Console.WriteLine("CREATED CONNECTION");
                    using (var channel = connection.CreateModel())
                    {
                        Console.WriteLine("CREATED CHANNEL");
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
                        
                        Console.WriteLine("ALL QUEUES DECLARED");

                        var temperatureConsumer =
                            new Consumer(channel, ConnectionString, DatabaseName, TemperatureCollection);
                        var humidityConsumer =
                            new Consumer(channel, ConnectionString, DatabaseName, HumidityCollection);
                        var windConsumer = new Consumer(channel, ConnectionString, DatabaseName, WindCollection);
                        var pressureConsumer =
                            new Consumer(channel, ConnectionString, DatabaseName, PressureCollection);

                        channel.BasicConsume(TemperatureCollection, false, temperatureConsumer);
                        channel.BasicConsume(HumidityCollection, false, humidityConsumer);
                        channel.BasicConsume(WindCollection, false, windConsumer);
                        channel.BasicConsume(PressureCollection, false, pressureConsumer);
                        Console.WriteLine("ALL CONSUMERS RUNNING");
                        while (true)
                        {
                            if (remainingMessages > 0)
                            {
                                Console.WriteLine("I'M IN LOOP");
                                remainingMessages--;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
