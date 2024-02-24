using System.Xml;
using TestTask.DAL.Entities;

namespace TestTask.DAL.XML;

public class InstrumentStatusReader
{
    private readonly XmlNode? InstrumentStatusNode;

    public InstrumentStatusReader(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        //XmlElement? root = doc.DocumentElement;
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

    public DeviceStatus ReadDeviceStatus(XmlNode node)
    {
        var moduleCategory = XmlWrapper.SelectSingle(node, "ModuleCategoryID").InnerText;
        return new DeviceStatus()
        {
            ModuleCategoryID = moduleCategory,
            IndexWithinRole = Int32.Parse(XmlWrapper.SelectSingle(node, "IndexWithinRole").InnerText),
            RapidControlStatus = ReadCombinedStatus(XmlWrapper.SelectSingle(node,"RapidControlStatus").InnerText, moduleCategory)
        };
    }

    public BaseCombinedStatus ReadCombinedStatus(string nodeInnerText, string category)
    {
        var status = new BaseCombinedStatus(
                moduleState: XmlExtractor.GetValueFromNode(nodeInnerText, "ModuleState"),
                isBusy: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsBusy")),
                isReady: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsReady")),
                isError: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsError")),
                keyLock: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "KeyLock")));

        switch(category){
            case "SAMPLER":
                status = new CombinedSamplerStatus(
                baseStatus: status,
                status: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Status")),
                vial: XmlExtractor.GetValueFromNode(nodeInnerText, "Vial"),
                volume: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Volume")),
                maximumInjectionVolume: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumInjectionVolume")),
                rackL: XmlExtractor.GetValueFromNode(nodeInnerText, "RackL"),
                rackR: XmlExtractor.GetValueFromNode(nodeInnerText, "RackR"),
                rackInf: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "RackInf")),
                buzzer: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Buzzer")));
                break;
            
            case "QUATPUMP":
                status = new CombinedPumpStatus(
                baseStatus: status,
                mode: XmlExtractor.GetValueFromNode(nodeInnerText, "Mode"),
                flow: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Flow")),
                percentB: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentB")),
                percentC: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentC")),
                percentD: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentD")),
                minimumPressureLimit: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MinimumPressureLimit")),
                maximumPressureLimit: Double.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumPressureLimit")),
                pressure: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Pressure")),
                pumpOn: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PumpOn")),
                channel: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Channel")));
                break;
            
            case "COLCOMP":
                status = new CombinedOvenStatus(
                baseStatus: status,
                useTemperatureControl: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "UseTemperatureControl")),
                ovenOn: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "OvenOn")),
                temperature_Actual: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Temperature_Actual")),
                temperature_Room: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Temperature_Room")),
                maximumTemperatureLimit: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumTemperatureLimit")),
                valve_Position: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Valve_Position")),
                valve_Rotations: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Valve_Rotations")),
                buzzer: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Buzzer")));
                break;
            }
        return status;
    }

}
