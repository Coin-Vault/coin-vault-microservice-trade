using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TradingService.Dtos;

namespace TradingService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly IMessageBusEncryption _messageBusEncryption;

        public MessageBusClient(IConfiguration configuration, IMessageBusEncryption messageBusEncryption)
        {
            _messageBusEncryption = messageBusEncryption;

            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration["RabbitMQUri"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Connected to RabbitMQ message bus");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Could not connect to RabbitMQ message bus: {exception.Message}");
            }
        }

        public void PublishNewTrade(TradePublishDto tradePublishDto)
        {
            var message = JsonSerializer.Serialize(tradePublishDto);

            message = _messageBusEncryption.EncryptMessage(_configuration["MessageEncryptionKey"], message);

            if (_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ connection open, sending message");
                SendMessage(message, "microservice1");
            }
            else
            {
                Console.WriteLine("RabbitMQ connection is closed, not sending");
            }
        }

        private void SendMessage(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            Console.WriteLine($"Published {message} to the portfolio service");
        }

        public void Dispose()
        {
            Console.WriteLine("RabbitMQ message bus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ was shutdown");
        }
    }
}
