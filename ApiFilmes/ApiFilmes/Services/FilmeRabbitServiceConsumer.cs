using Data.Redis.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ApiFilmes.Services
{

    /// <summary>
    /// Serviço rodando em background para salvar no banco os logs gerados pelo queue.
    /// </summary>
    public class FilmeRabbitServiceConsumer : BackgroundService
    { 
        private readonly ILogger<FilmeRabbitServiceConsumer> _logger;
        private FilmesServices _filmesServices = new FilmesServices();
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private string _ipHost = "172.17.0.4"; //ip do docker

        public FilmeRabbitServiceConsumer(ILogger<FilmeRabbitServiceConsumer> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _ipHost
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "FilmesController",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var _message = Encoding.UTF8.GetString(body);

                    //confirmação da leitura
                    _channel.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation("Recebido {0}", _message);
                    _filmesServices.repositorioFilmes.SaveLogFilmesBuscados(_message, $"Logger {Guid.NewGuid().ToString()}");
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true); // caso caia em uma ex, jogo de volta na fila pra evitar que perca a info.
                    _logger.LogInformation("Falhou consumo");
                }

            };

            _channel.BasicConsume(queue: "FilmesController",
                                 autoAck: false, //mantém false para ser tratado no try catch a confirmação da leitura
                                 consumer: consumer);


            return Task.CompletedTask;
        }
         
         
    }

}
