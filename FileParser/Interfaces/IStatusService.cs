using FileParser.Entities;

namespace FileParser.Interfaces;

public interface IStatusService : IDisposable
{
    InstrumentStatus ChangeModuleState(ref InstrumentStatus instrumentStatus);
    Task<InstrumentStatus> ReadInstrumentStatusAsync();
    void PublishInstrumentStatus(InstrumentStatus instrumentStatus);
}
