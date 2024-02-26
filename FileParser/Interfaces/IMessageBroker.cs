namespace FileParser.Interfaces;

public interface IMessageBroker
{
    void Publish(string json);
}
