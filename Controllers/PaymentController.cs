using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("send-payment")]
    public IActionResult SendPayment([FromBody] PaymentRequest request)
    {
        var rabbitMQ = new RabbitMQHelper("PaymentQueue");

        string message = JsonConvert.SerializeObject(request);
        rabbitMQ.SendMessage(message);
        rabbitMQ.Close();

        return Ok("Payment added to the queue.");
    }
}

public class PaymentRequest
{
    public string User { get; set; }
    public string PaymentType { get; set; }
    public string CardNo { get; set; }
}
