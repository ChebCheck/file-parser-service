using FileParser.Entities;

namespace FileParser.Interfaces;

public interface IStatusReader : IDisposable
{
    InstrumentStatus Read();
    Task<InstrumentStatus> ReadAsync();
}
