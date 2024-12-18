
using RabbitMQ.Client;
using System.Text;

public class RabbitMQHelper
{
    private readonly string _hostname = "localhost";
    private readonly string _queueName;
    private readonly IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;

    public RabbitMQHelper(string queueName)
    {
        _queueName = queueName;
        var factory = new ConnectionFactory() { HostName = _hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "",
                              routingKey: _queueName,
                              basicProperties: null,
                              body: body);
    }

    public void Close()
    {
        _channel.Close(); 
        _connection.Close();
    }
}
