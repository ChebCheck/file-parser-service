using FileParser.Interfaces;
using Microsoft.Extensions.Hosting;

namespace FileParser.Services;

public class MainHostedService : BackgroundService
{
    private readonly IStatusService _service;

    public MainHostedService(IStatusService service) 
    {
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var instrumentStatus = await _service.ReadInstrumentStatusAsync();
            _service.ChangeModuleState(ref instrumentStatus);
            _service.PublishInstrumentStatus(instrumentStatus);
            await Task.Delay(1000);
        }
    }
}
