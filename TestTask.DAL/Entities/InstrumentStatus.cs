namespace FileParser.Entities;

public  class InstrumentStatus
{
    public string? PackageID { get; set; }
    public List<DeviceStatus> DeviceStatuses { get; set; } = new List<DeviceStatus>();
}
