using FileParser.Entities;
using FileParser.Interfaces;
using System.Text.Json;

namespace FileParser.Services;

public class InstrumentStatusService : IStatusService
{
    private readonly IStatusReader _statusReader;
    private readonly IMessageBroker _messageBroker;

    public InstrumentStatusService(IStatusReader statusReader, IMessageBroker messageBroker)
    {
        _statusReader = statusReader;
        _messageBroker = messageBroker;
    }

    public InstrumentStatus ChangeModuleState(ref InstrumentStatus instrumentStatus)
    {
        foreach(DeviceStatus deviceStat in instrumentStatus.DeviceStatuses)
        {
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
}
