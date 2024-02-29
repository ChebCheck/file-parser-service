using FileParser.Entities;
using FileParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FileParser.Services;

public class InstrumentStatusService : IStatusService
{
    private readonly IStatusReader _statusReader;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger _logger;

    public InstrumentStatusService(ILogger<InstrumentStatusService> logger, IStatusReader statusReader, IMessageBroker messageBroker)
    {
        _statusReader = statusReader;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public InstrumentStatus ChangeModuleState(ref InstrumentStatus instrumentStatus)
    {
        foreach(DeviceStatus deviceStat in instrumentStatus.DeviceStatuses)
        {
            if (deviceStat.RapidControlStatus == null)
            {
                _logger.LogError($"RapidControlStatus of {deviceStat.ModuleCategoryID} device is not defined.");
                throw new Exception();
            }
            deviceStat.RapidControlStatus.ModuleState = new Random().Next(4) switch
            {
                0 => "Online",
                1 => "Run",
                2 => "NotReady",
                _ => "Offline"
            };
        }

        return instrumentStatus;
    }

    public void PublishInstrumentStatus(InstrumentStatus instrumentStatus)
    {
        var json = JsonSerializer.Serialize(instrumentStatus, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        _messageBroker.Publish(json);
    }

    public Task<InstrumentStatus> ReadInstrumentStatusAsync()
    {
        return _statusReader.ReadAsync();
    }

    public void Dispose()
    {
        _messageBroker.Dispose();
    }
}
