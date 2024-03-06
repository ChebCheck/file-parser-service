using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using FileParser.DI;
using Microsoft.Extensions.Hosting;

ServiceCollection services = new ServiceCollection();
services.AddApplicationServices(ConfigurationManager.AppSettings);

var serviceProvider = services.BuildServiceProvider();
var hostedService = serviceProvider.GetRequiredService<BackgroundService>();

var tokenSource = new CancellationTokenSource();
await hostedService.StartAsync(tokenSource.Token);

Console.WriteLine("Press [enter] to exit");
Console.ReadLine();

tokenSource.Cancel();
tokenSource.Dispose();