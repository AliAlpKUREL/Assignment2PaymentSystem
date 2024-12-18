using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    [HttpPost("send-notification")]
    public IActionResult SendNotification()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "NotificationQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var notification = JsonConvert.DeserializeObject<dynamic>(message);

           
            Console.WriteLine($"Notification Sent: {notification.user}, Message : {notification.message}");
        };

        channel.BasicConsume(queue: "NotificationQueue",
                             autoAck: true,
                             consumer: consumer);

        return Ok("Notification has started to be sent.\r\n");
    }

}
