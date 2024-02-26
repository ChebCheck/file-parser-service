using FileParser.Interfaces;
using FileParser.Readers;
using FileParser.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace FileParser.DI;
public static class ServiceRegister
{
    public static void AddContracts(this IServiceCollection services, string producerQueueKey)
    {
        services.AddScoped<IStatusReader, InstrumentStatusReader>((provider) => new InstrumentStatusReader("../../../Source/status.xml"));
        services.AddScoped<IStatusService, InstrumentStatusService>();
        services.AddScoped<IMessageBroker, MessageProducer>((provider) => new MessageProducer(producerQueueKey));
    }
}
