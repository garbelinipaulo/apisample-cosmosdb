using RabbitMQ.Client;
using System.Text;

namespace Services.Rabbit
{
    public static class QueueChannel
    {

#if Release
        const string configHost = "connRemota";
#else
        const string configHost = "localhost";
#endif

        public static async void GerarQueue(string xTipo, string Key)
        {
            var factory = new ConnectionFactory() { HostName = configHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: xTipo,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                 
                var body = Encoding.UTF8.GetBytes(Key);

                channel.BasicPublish(exchange: "",
                                     routingKey: xTipo,
                                     basicProperties: null,
                                     body: body); 
            }
        }
    }
}
