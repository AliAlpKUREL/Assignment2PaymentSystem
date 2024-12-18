
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client.Events;

namespace Assigment2Payment
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProcessorController : ControllerBase
    {
        [HttpPost("process-payment")]
        public IActionResult ProcessPayment()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "PaymentQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var payment = JsonConvert.DeserializeObject<PaymentRequest>(message);

               
                var notificationMessage = new
                {
                    user = payment.User,
                    message = "Your payment has been processed successfully."
                };

                var notificationQueue = new RabbitMQHelper("NotificationQueue");
                notificationQueue.SendMessage(JsonConvert.SerializeObject(notificationMessage));
                notificationQueue.Close();
            };

            channel.BasicConsume(queue: "PaymentQueue",
                                 autoAck: true,
                                 consumer: consumer);

            return Ok("Payment processing has begun.");
        }

    }
}
