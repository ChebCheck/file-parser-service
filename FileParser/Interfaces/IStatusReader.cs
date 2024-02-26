using FileParser.Entities;

namespace FileParser.Interfaces;

public interface IStatusReader
{
    InstrumentStatus Read();
    Task<InstrumentStatus> ReadAsync();
}
