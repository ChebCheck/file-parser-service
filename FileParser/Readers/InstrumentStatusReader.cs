using System.Xml;
using System.Xml.Serialization;
using FileParser.Entities;
using FileParser.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileParser.Readers;

public class InstrumentStatusReader : IStatusReader
{
    private readonly FileStream stream;
    private readonly ILogger _logger;

    public InstrumentStatusReader(ILogger<InstrumentStatusReader> logger, string path)
    {
        _logger = logger;
        if(path == null)
        {
            _logger.LogError("Path to source file is null.");
            throw new ArgumentNullException(nameof(path), "Argument must be not null");
        }

        stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    }

    public InstrumentStatus Read()
    {
        var serializer = new XmlSerializer(typeof(InstrumentStatus));
        using var reader = XmlReader.Create(stream);

        var InstrumentStatusEntity = serializer.Deserialize(reader) as InstrumentStatus;
        stream.Position = 0;

        if (InstrumentStatusEntity == null)
        {
            _logger.LogError("Instrument status is can't be read.");
            throw new Exception();
        }

        foreach (DeviceStatus deviceStatus in InstrumentStatusEntity.DeviceStatuses)
        {
            var result = ReadCombinedStatus(deviceStatus.ModuleCategoryID);
            if(result == null)
            {
                _logger.LogError("Combined status is can't be readed.");
                throw new Exception();
            }
            deviceStatus.RapidControlStatus = result;
        }
        
        return InstrumentStatusEntity;
    }

    public Task<InstrumentStatus> ReadAsync()
    {
        return Task.Run(() => Read());
    }

    private BaseCombinedStatus? ReadCombinedStatus(string category)
    {
        using var reader = XmlReader.Create(stream, new XmlReaderSettings()
        {
            IgnoreWhitespace = true,
        });

        switch (category)
        {
            case "SAMPLER":
                return ReadStatusByName<CombinedSamplerStatus>(reader, "CombinedSamplerStatus");

            case "QUATPUMP":
                return ReadStatusByName<CombinedPumpStatus>(reader, "CombinedPumpStatus");

            case "COLCOMP":
                return ReadStatusByName<CombinedOvenStatus>(reader, "CombinedOvenStatus");

            default:
                return new BaseCombinedStatus();
        }
    }

    private NodeType? ReadStatusByName<NodeType>(XmlReader reader, string deviceType) where NodeType : BaseCombinedStatus
    {
        while (reader.Read())
        {
            if (reader.Name == "RapidControlStatus")
            {
                var innerXml = reader.ReadInnerXml();
                innerXml = innerXml.Replace("&lt;", "<");
                innerXml = innerXml.Replace("&gt;", ">");
                using var textReader = new StringReader(innerXml);
                var serializer = new XmlSerializer(typeof(NodeType));
                if(IsCorrectCurrentNode(innerXml, deviceType))
                {
                    stream.Position = 0;
                    return serializer.Deserialize(textReader) as NodeType;
                }
            }
        }
        return null;
    }

    private bool IsCorrectCurrentNode(string xmlText, string nodeName)
    {
        _logger.LogInformation($"Try to find {nodeName}");
        if (xmlText.Contains(nodeName))
        {
            _logger.LogInformation("Success");
            return true;
        }
        _logger.LogInformation("Failed");
        return false;
    }

    public void Dispose()
    {
        stream.Close();
    }
}
