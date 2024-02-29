using FileParser.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace FileParser.Services;

public class MessageProducer : IMessageBroker
{
    private readonly ILogger _logger;
    private readonly IModel channel;
    private readonly IConnection connection;
    private readonly string queueKey = "InstrumentStatus";

    public MessageProducer(ILogger<MessageProducer> logger, ConnectionFactory factory)
    {
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.QueueDeclare(queueKey, false, false, false, null);
        _logger = logger;
    }

    public void Publish(string json)
    {
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(string.Empty, queueKey, null, body);
        _logger.LogInformation("Published");
    }

    public void Dispose()
    {
        channel.Close();
        connection.Close();
    }
}
