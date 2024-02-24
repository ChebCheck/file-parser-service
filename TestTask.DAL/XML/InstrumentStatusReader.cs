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
        Console.WriteLine(XmlWrapper.SelectSingle(node, "RapidControlStatus").InnerText);
        return new DeviceStatus()
        {
            ModuleCategoryID = moduleCategory,
            IndexWithinRole = Int32.Parse(XmlWrapper.SelectSingle(node, "IndexWithinRole").InnerText),
            RapidControlStatus = ReadCombinedStatus(XmlWrapper.SelectSingle(node,"RapidControlStatus"), moduleCategory)
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
                    pressure: Int32.Parse(XmlWrapper.SelectSingle(node, "Presure").InnerText),
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
                    maximumTemperatureLimit: float.Parse(XmlWrapper.SelectSingle(node, "maximumTemperatureLimit").InnerText),
                    valve_Position: Int32.Parse(XmlWrapper.SelectSingle(node, "Valve_Position").InnerText),
                    valve_Rotations: Int32.Parse(XmlWrapper.SelectSingle(node, "valve_Rotations").InnerText),
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

}
