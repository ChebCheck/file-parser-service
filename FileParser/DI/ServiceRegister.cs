using FileParser.Interfaces;
using FileParser.Readers;
using FileParser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;

namespace FileParser.DI;
public static class ServiceRegister
{
    public static void AddApplicationServices(this IServiceCollection services, NameValueCollection appSettings)
    {
        services.AddLogging((configure) => configure.AddConsole());

        services.AddScoped<IStatusReader, InstrumentStatusReader>(
            (provider) => new InstrumentStatusReader(
                provider.GetRequiredService<ILogger<InstrumentStatusReader>>(), 
                appSettings["sourceFile"]));
        
        services.AddScoped<IStatusService, InstrumentStatusService>();
        
        services.AddScoped<IMessageBroker, MessageProducer>(
            (provider) => new MessageProducer(
                provider.GetRequiredService<ILogger<MessageProducer>>(), 
                new RabbitMQ.Client.ConnectionFactory()
                {
                    HostName = appSettings["hostName"],
                    VirtualHost = appSettings["virtualHost"],
                    Port = Int32.Parse(appSettings["port"]),
                    UserName = appSettings["username"],
                    Password = appSettings["password"]
                }));
        
        services.AddScoped<BackgroundService, MainHostedService>();
    }
}
