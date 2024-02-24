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

    public BaseCombinedStatus ReadCombinedStatus(XmlNode node, string category)
    {
        var status = category switch
        {
            "SAMPLER" => new CombinedSamplerStatus(
                    moduleState: XmlWrapper.SelectSingle(node, "ModuleState").InnerText,
                    isBusy: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsBusy").InnerText),
                    isReady: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsReady").InnerText),
                    isError: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsError").InnerText),
                    keyLock: Boolean.Parse(XmlWrapper.SelectSingle(node, "KeyLock").InnerText),
                    status: Int32.Parse(XmlWrapper.SelectSingle(node, "Status").InnerText),
                    vial: XmlWrapper.SelectSingle(node, "Vial").InnerText,
                    volume: Int32.Parse(XmlWrapper.SelectSingle(node, "Volume").InnerText),
                    maximumInjectionVolume: Int32.Parse(XmlWrapper.SelectSingle(node, "MaximumInjectionVolume").InnerText),
                    rackL: XmlWrapper.SelectSingle(node, "RackL").InnerText,
                    rackR: XmlWrapper.SelectSingle(node, "RackR").InnerText,
                    rackInf: Int32.Parse(XmlWrapper.SelectSingle(node, "RackInf").InnerText),
                    buzzer: Boolean.Parse(XmlWrapper.SelectSingle(node, "Buzzer").InnerText)
                ),
            "QUATPUMP" => new CombinedPumpStatus(
                    moduleState: XmlWrapper.SelectSingle(node, "ModuleState").InnerText,
                    isBusy: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsBusy").InnerText),
                    isReady: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsReady").InnerText),
                    isError: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsError").InnerText),
                    keyLock: Boolean.Parse(XmlWrapper.SelectSingle(node, "KeyLock").InnerText),
                    mode: XmlWrapper.SelectSingle(node, "Mode").InnerText,
                    flow: Int32.Parse(XmlWrapper.SelectSingle(node, "Flow").InnerText),
                    percentB: Int32.Parse(XmlWrapper.SelectSingle(node, "PercentB").InnerText),
                    percentC: Int32.Parse(XmlWrapper.SelectSingle(node, "PercentC").InnerText),
                    percentD: Int32.Parse(XmlWrapper.SelectSingle(node, "PercentD").InnerText),
                    minimumPressureLimit: Int32.Parse(XmlWrapper.SelectSingle(node, "MinimumPressureLimit").InnerText),
                    maximumPressureLimit: Double.Parse(XmlWrapper.SelectSingle(node, "MaximumPressureLimit").InnerText),
                    pressure: Int32.Parse(XmlWrapper.SelectSingle(node, "Pressure").InnerText),
                    pumpOn: Boolean.Parse(XmlWrapper.SelectSingle(node, "PumpOn").InnerText),
                    channel: Int32.Parse(XmlWrapper.SelectSingle(node, "Channel").InnerText)
                ),
            "COLCOMP" => new CombinedOvenStatus(
                    moduleState: XmlWrapper.SelectSingle(node, "ModuleState").InnerText,
                    isBusy: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsBusy").InnerText),
                    isReady: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsReady").InnerText),
                    isError: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsError").InnerText),
                    keyLock: Boolean.Parse(XmlWrapper.SelectSingle(node, "KeyLock").InnerText),
                    useTemperatureControl: Boolean.Parse(XmlWrapper.SelectSingle(node, "UseTemperatureControl").InnerText),
                    ovenOn: Boolean.Parse(XmlWrapper.SelectSingle(node, "OvenOn").InnerText),
                    temperature_Actual: float.Parse(XmlWrapper.SelectSingle(node, "Temperature_Actual").InnerText),
                    temperature_Room: float.Parse(XmlWrapper.SelectSingle(node, "Temperature_Room").InnerText),
                    maximumTemperatureLimit: float.Parse(XmlWrapper.SelectSingle(node, "MaximumTemperatureLimit").InnerText),
                    valve_Position: Int32.Parse(XmlWrapper.SelectSingle(node, "Valve_Position").InnerText),
                    valve_Rotations: Int32.Parse(XmlWrapper.SelectSingle(node, "Valve_Rotations").InnerText),
                    buzzer: Boolean.Parse(XmlWrapper.SelectSingle(node, "Buzzer").InnerText)
                ),
            _ => new BaseCombinedStatus(
                    moduleState: XmlWrapper.SelectSingle(node, "ModuleState").InnerText,
                    isBusy: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsBusy").InnerText),
                    isReady: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsReady").InnerText),
                    isError: Boolean.Parse(XmlWrapper.SelectSingle(node, "IsError").InnerText),
                    keyLock: Boolean.Parse(XmlWrapper.SelectSingle(node, "KeyLock").InnerText)
                )
        };
        return status;
    }

    public BaseCombinedStatus ReadCombinedStatus(string nodeInnerText, string category)
    {
        var status = category switch
        {
            "SAMPLER" => new CombinedSamplerStatus(
                    moduleState: XmlExtractor.GetValueFromNode(nodeInnerText, "ModuleState"),
                    isBusy: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsBusy")),
                    isReady: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsReady")),
                    isError: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsError")),
                    keyLock: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "KeyLock")),
                    status: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Status")),
                    vial: XmlExtractor.GetValueFromNode(nodeInnerText, "Vial"),
                    volume: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Volume")),
                    maximumInjectionVolume: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumInjectionVolume")),
                    rackL: XmlExtractor.GetValueFromNode(nodeInnerText, "RackL"),
                    rackR: XmlExtractor.GetValueFromNode(nodeInnerText, "RackR"),
                    rackInf: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "RackInf")),
                    buzzer: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Buzzer"))
                ),
            "QUATPUMP" => new CombinedPumpStatus(
                    moduleState: XmlExtractor.GetValueFromNode(nodeInnerText, "ModuleState"),
                    isBusy: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsBusy")),
                    isReady: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsReady")),
                    isError: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsError")),
                    keyLock: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "KeyLock")),
                    mode: XmlExtractor.GetValueFromNode(nodeInnerText, "Mode"),
                    flow: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Flow")),
                    percentB: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentB")),
                    percentC: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentC")),
                    percentD: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PercentD")),
                    minimumPressureLimit: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MinimumPressureLimit")),
                    maximumPressureLimit: Double.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumPressureLimit")),
                    pressure: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Pressure")),
                    pumpOn: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "PumpOn")),
                    channel: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Channel"))
                ),
            "COLCOMP" => new CombinedOvenStatus(
                    moduleState: XmlExtractor.GetValueFromNode(nodeInnerText, "ModuleState"),
                    isBusy: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsBusy")),
                    isReady: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsReady")),
                    isError: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsError")),
                    keyLock: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "KeyLock")),
                    useTemperatureControl: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "UseTemperatureControl")),
                    ovenOn: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "OvenOn")),
                    temperature_Actual: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Temperature_Actual")),
                    temperature_Room: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Temperature_Room")),
                    maximumTemperatureLimit: float.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "MaximumTemperatureLimit")),
                    valve_Position: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Valve_Position")),
                    valve_Rotations: Int32.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Valve_Rotations")),
                    buzzer: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "Buzzer"))
                ),
            _ => new BaseCombinedStatus(
                    moduleState: XmlExtractor.GetValueFromNode(nodeInnerText, "ModuleState"),
                    isBusy: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsBusy")),
                    isReady: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsReady")),
                    isError: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "IsError")),
                    keyLock: Boolean.Parse(XmlExtractor.GetValueFromNode(nodeInnerText, "KeyLock"))
                )
        };
        return status;
    }

}
