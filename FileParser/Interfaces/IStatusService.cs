using FileParser.Entities;

namespace FileParser.Interfaces;

public interface IStatusService
{
    InstrumentStatus ChangeModuleState(ref InstrumentStatus instrumentStatus);
    Task<InstrumentStatus> ReadInstrumentStatusAsync();
    void PublishInstrumentStatus(InstrumentStatus instrumentStatus);
}
