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

    public InstrumentStatus ChangeModuleState(ref InstrumentStatus instrumentStat)
    {
        foreach(DeviceStatus deviceStat in instrumentStat.DeviceStatuses)
        {
            deviceStat.RapidControlStatus.ModuleState = new Random().Next(4) switch
            {
                0 => "Online",
                1 => "Run",
                2 => "NotReady",
                3 => "Offline"
            };
        }

        return instrumentStat;
    }

    public void PublishInstrumentStatus(InstrumentStatus instrumentStatus)
    {
        var json = JsonSerializer.Serialize(instrumentStatus, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        Console.WriteLine(json);
        _messageBroker.Publish(json);
    }

    public async Task<InstrumentStatus> ReadInstrumentStatusAsync()
    {
        return await _statusReader.ReadAsync();
    }
}
