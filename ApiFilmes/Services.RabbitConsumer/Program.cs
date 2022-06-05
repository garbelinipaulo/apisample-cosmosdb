using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

 class Program
{

#if Release
        const string configHost = "connRemota";
#else
    const string configHost = "localhost";
#endif
      
    static void Main(string[] args)
    {

        List<string> _listaEventosConsumidos = new List<string>();
        var factory = new ConnectionFactory() { HostName = configHost };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "FilmesController",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var _message = Encoding.UTF8.GetString(body);

                    //confirmação da leitura
                    channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine(" [x] Recebido {0}", _message);
                }
                catch (Exception ex)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true); // caso caia em uma ex, jogo de volta na fila pra evitar que perca a info.
                }

            };

            channel.BasicConsume(queue: "FilmesController",
                                 autoAck: false, //mantém false para ser tratado no try catch a confirmação da leitura
                                 consumer: consumer);

            Console.WriteLine("Aperte enter para sair");
            Console.ReadLine();
        }
    }

}
