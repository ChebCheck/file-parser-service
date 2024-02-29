namespace FileParser.Interfaces;

public interface IMessageBroker : IDisposable
{
    void Publish(string json);
}
