using System.Text.RegularExpressions;
using System.Xml;
using FileParser.Entities;
using FileParser.Interfaces;

namespace FileParser.Readers;

public class InstrumentStatusReader : IStatusReader
{
    private readonly XmlNode? InstrumentStatusNode;

    public InstrumentStatusReader(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        InstrumentStatusNode = doc.DocumentElement;
    }

    public InstrumentStatus Read()
    {
        var InstrumentStatusEntity = new InstrumentStatus();
        if (InstrumentStatusNode != null)
        {
            InstrumentStatusEntity.PackageID = XmlWrapper.SelectSingle(InstrumentStatusNode, "PackageID").InnerText;
            var DeviceStatusNodes = XmlWrapper.SelectAny(InstrumentStatusNode, "DeviceStatus");
            foreach (XmlNode node in DeviceStatusNodes)
            {
                InstrumentStatusEntity.DeviceStatuses.Add(ReadDeviceStatus(node));
            }
        }
        return InstrumentStatusEntity;
    }

    public async Task<InstrumentStatus> ReadAsync()
    {
        return await Task.Run(() => Read());
    }

    private DeviceStatus ReadDeviceStatus(XmlNode node)
    {
        var moduleCategory = XmlWrapper.SelectSingle(node, "ModuleCategoryID").InnerText;
        return new DeviceStatus()
        {
            ModuleCategoryID = moduleCategory,
            IndexWithinRole = Int32.Parse(XmlWrapper.SelectSingle(node, "IndexWithinRole").InnerText),
            RapidControlStatus = ReadCombinedStatus(XmlWrapper.SelectSingle(node,"RapidControlStatus").InnerText, moduleCategory)
        };
    }

    private BaseCombinedStatus ReadCombinedStatus(string nodeInnerText, string category)
    {
        var status = new BaseCombinedStatus(
                moduleState: ReadNodeFromText(nodeInnerText, "ModuleState"),
                isBusy: Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsBusy")),
                isReady: Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsReady")),
                isError: Boolean.Parse(ReadNodeFromText(nodeInnerText, "IsError")),
                keyLock: Boolean.Parse(ReadNodeFromText(nodeInnerText, "KeyLock")));

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

    private string ReadNodeFromText(string xmlText, string nodeName)
    {
        Console.WriteLine($"[*] Try to extract {nodeName}");
        Regex regEx = new Regex($@"<{nodeName}>(\S+)</{nodeName}>");
        string nodeText = regEx.Match(xmlText).Value;
        int valueLength = nodeText.LastIndexOf('<') - 1 - nodeText.IndexOf('>');
        Console.WriteLine("[V] Success\n");
        return nodeText.Substring(nodeText.IndexOf('>') + 1, valueLength);
    }
}
