using Microsoft.Extensions.DependencyInjection;
using FileParser.DI;
using FileParser.Interfaces;

ServiceCollection services = new ServiceCollection();
services.AddServices("InstrumentStatus");

var serviceProvider = services.BuildServiceProvider();
var instrumentService = serviceProvider.GetRequiredService<IStatusService>();

CancellationToken token = new CancellationToken();
Task task = Task.Run(async () =>
{
    while (true)
    {
        var instrumentStatus = await instrumentService.ReadInstrumentStatusAsync();
        instrumentService.ChangeModuleState(ref instrumentStatus);
        instrumentService.PublishInstrumentStatus(instrumentStatus);
        Thread.Sleep(1000);
    }
}, token);

Console.WriteLine("Pres any key to exit");
Console.ReadKey();
token = new CancellationToken(true);