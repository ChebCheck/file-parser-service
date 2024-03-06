using System.Xml.Serialization;

namespace FileParser.Entities;

public  class InstrumentStatus
{
    public string? PackageID { get; set; }

    [XmlElement(ElementName = "DeviceStatus")]
    public List<DeviceStatus> DeviceStatuses { get; set; } = new List<DeviceStatus>();

    public InstrumentStatus() { }
}
