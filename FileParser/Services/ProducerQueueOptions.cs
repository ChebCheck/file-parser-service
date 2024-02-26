namespace FileParser.Services;

public class ProducerQueueOptions
{
    public bool durable { get; set; } = false;
    public bool exclusive { get; set; } = false;
    public bool autoDelete { get; set; } = false;
    public IDictionary<string, object>? arguments { get; set; } = null;
    
    public ProducerQueueOptions() { }

    public ProducerQueueOptions(bool durable, bool exclusive, bool autoDelete, IDictionary<string, object>? arguments)
    {
        this.durable = durable;
        this.exclusive = exclusive;
        this.autoDelete = autoDelete;
        this.arguments = arguments;
    }
}
