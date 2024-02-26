using FileParser.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace FileParser.Services;

public class MessageProducer : IMessageBroker
{
    private readonly IModel channel;
    private readonly IConnection connection;
    private readonly string queueKey;
    public ProducerQueueOptions QueueOptions { get; set; }
    public IBasicProperties? PublishProperties { get; set; }

    public MessageProducer(string queueKey, string hostName = "localhost")
    {
        QueueOptions = new ProducerQueueOptions();
        PublishProperties = null;

        var factory = new ConnectionFactory() { HostName = hostName };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.QueueDeclare(queueKey, QueueOptions.durable, QueueOptions.exclusive, QueueOptions.autoDelete, QueueOptions.arguments);

        this.queueKey = queueKey;
    }

    public void Publish(string json)
    {
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(string.Empty, queueKey, PublishProperties, body);
    }

    ~ MessageProducer()
    {
        channel.Dispose();
        connection.Dispose();
    }
}
