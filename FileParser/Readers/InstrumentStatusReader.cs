using System.Text.RegularExpressions;
using System.Xml;
using FileParser.Entities;
using FileParser.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileParser.Readers;

public class InstrumentStatusReader : IStatusReader
{
    private readonly XmlNode? InstrumentStatusNode;
    private readonly ILogger _logger;

    public InstrumentStatusReader(ILogger<InstrumentStatusReader> logger, string path)
    {
        _logger = logger;
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        InstrumentStatusNode = doc.DocumentElement;
    }

    public InstrumentStatus Read()
    {
        var InstrumentStatusEntity = new InstrumentStatus();
        if (InstrumentStatusNode != null)
        {
            InstrumentStatusEntity.PackageID = SelectSingle(InstrumentStatusNode, "PackageID").InnerText;
            var DeviceStatusNodes = SelectAny(InstrumentStatusNode, "DeviceStatus");
            foreach (XmlNode node in DeviceStatusNodes)
            {
                InstrumentStatusEntity.DeviceStatuses.Add(ReadDeviceStatus(node));
            }
        }
        return InstrumentStatusEntity;
    }

    public Task<InstrumentStatus> ReadAsync()
    {
        return Task.Run(() => Read());
    }

    private DeviceStatus ReadDeviceStatus(XmlNode node)
    {
        var moduleCategory = SelectSingle(node, "ModuleCategoryID").InnerText;
        return new DeviceStatus()
        {
            ModuleCategoryID = moduleCategory,
            IndexWithinRole = Int32.Parse(SelectSingle(node, "IndexWithinRole").InnerText),
            RapidControlStatus = ReadCombinedStatus(SelectSingle(node,"RapidControlStatus").InnerText, moduleCategory)
        };
    }

    private BaseCombinedStatus ReadCombinedStatus(string nodeInnerText, string category)
    {
        var status = new BaseCombinedStatus() {
                ModuleState = ReadNodeFromText(nodeInnerText, "ModuleState"),
                IsBusy = Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsBusy")),
                IsReady = Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsReady")),
                IsError = Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsError")),
                KeyLock = Boolean.Parse(ReadNodeFromText(nodeInnerText, "KeyLock"))};

        switch(category){
            case "SAMPLER":
                status = new CombinedSamplerStatus(
                baseStatus: status,
                status: Int32.Parse(ReadNodeFromText(nodeInnerText, "Status")),
                vial: ReadNodeFromText(nodeInnerText, "Vial"),
                volume: Int32.Parse(ReadNodeFromText(nodeInnerText, "Volume")),
                maximumInjectionVolume: Int32.Parse(ReadNodeFromText(nodeInnerText, "MaximumInjectionVolume")),
                rackL: ReadNodeFromText(nodeInnerText, "RackL"),
                rackR: ReadNodeFromText(nodeInnerText, "RackR"),
                rackInf: Int32.Parse(ReadNodeFromText(nodeInnerText, "RackInf")),
                buzzer: Boolean.Parse(ReadNodeFromText(nodeInnerText, "Buzzer")));
                break;
            
            case "QUATPUMP":
                status = new CombinedPumpStatus(
                baseStatus: status,
                mode: ReadNodeFromText(nodeInnerText, "Mode"),
                flow: Int32.Parse(ReadNodeFromText(nodeInnerText, "Flow")),
                percentB: Int32.Parse(ReadNodeFromText(nodeInnerText, "PercentB")),
                percentC: Int32.Parse(ReadNodeFromText(nodeInnerText, "PercentC")),
                percentD: Int32.Parse(ReadNodeFromText(nodeInnerText, "PercentD")),
                minimumPressureLimit: Int32.Parse(ReadNodeFromText(nodeInnerText, "MinimumPressureLimit")),
                maximumPressureLimit: Double.Parse(ReadNodeFromText(nodeInnerText, "MaximumPressureLimit")),
                pressure: Int32.Parse(ReadNodeFromText(nodeInnerText, "Pressure")),
                pumpOn: Boolean.Parse(ReadNodeFromText(nodeInnerText, "PumpOn")),
                channel: Int32.Parse(ReadNodeFromText(nodeInnerText, "Channel")));
                break;
            
            case "COLCOMP":
                status = new CombinedOvenStatus(
                baseStatus: status,
                useTemperatureControl: Boolean.Parse(ReadNodeFromText(nodeInnerText, "UseTemperatureControl")),
                ovenOn: Boolean.Parse(ReadNodeFromText(nodeInnerText, "OvenOn")),
                temperature_Actual: float.Parse(ReadNodeFromText(nodeInnerText, "Temperature_Actual")),
                temperature_Room: float.Parse(ReadNodeFromText(nodeInnerText, "Temperature_Room")),
                maximumTemperatureLimit: float.Parse(ReadNodeFromText(nodeInnerText, "MaximumTemperatureLimit")),
                valve_Position: Int32.Parse(ReadNodeFromText(nodeInnerText, "Valve_Position")),
                valve_Rotations: Int32.Parse(ReadNodeFromText(nodeInnerText, "Valve_Rotations")),
                buzzer: Boolean.Parse(ReadNodeFromText(nodeInnerText, "Buzzer")));
                break;
            }
        return status;
    }

    public XmlNode SelectSingle(XmlNode context, string xPath)
    {
        _logger.LogInformation($"Trying to read {xPath} from {context.Name}");
        var result = context.SelectSingleNode(xPath);
        if (result == null)
        {
            throw new Exception();
        }
        _logger.LogInformation("Success\n");
        return result;
    }

    public XmlNodeList SelectAny(XmlNode context, string xPath)
    {
        _logger.LogInformation($"Trying to read {xPath} from {context.Name}");
        var result = context.SelectNodes(xPath);
        if (result == null)
        {
            throw new Exception();
        }
        _logger.LogInformation("Success\n");
        return result;
    }

    private string ReadNodeFromText(string xmlText, string nodeName)
    {
        _logger.LogInformation($"Try to extract {nodeName}");
        Regex regEx = new Regex($@"<{nodeName}>(\S+)</{nodeName}>");
        string nodeText = regEx.Match(xmlText).Value;
        int valueLength = nodeText.LastIndexOf('<') - 1 - nodeText.IndexOf('>');
        _logger.LogInformation("Success\n");
        return nodeText.Substring(nodeText.IndexOf('>') + 1, valueLength);
    }
}
