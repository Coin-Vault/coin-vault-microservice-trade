using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TradingService.Dtos;

namespace TradingService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuartion;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly IMessageBusEncryption _messageBusEncryption;

        public MessageBusClient(IConfiguration configuration, IMessageBusEncryption messageBusEncryption)
        {
            _messageBusEncryption = messageBusEncryption;

            _configuartion = configuration;
            var factory = new ConnectionFactory() { 
                Uri = new Uri(_configuartion["RabbitMQUri"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Connected to RabbitMQ messagebus");
            }   
            catch(Exception exeption)
            {
                Console.WriteLine($"Could not connect to RabbitMQ messagebus: {exeption.Message}");
            } 
        }

        public void PublishNewTrade(TradePublishDto tradePublishDto)
        {
            var Message = JsonSerializer.Serialize(tradePublishDto);

            Message = _messageBusEncryption.EncryptMessage(_configuartion["MessageEncryptionKey"], Message);

            if(_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ connection open, sending message");
                SendMessage(Message);
            }
            else {
                Console.WriteLine("RabbitMQ connection is closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", 
                routingKey: "",
                basicProperties: null,
                body: body);

            Console.WriteLine($"Published {message} to the portfolio service");
        }

        public void Dispose() 
        {
            Console.WriteLine("RabbitMQ messagebus disposed");
            if(_channel.IsOpen)
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