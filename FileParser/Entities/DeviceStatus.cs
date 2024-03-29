﻿using System.Xml.Serialization;

namespace FileParser.Entities;

public class DeviceStatus
{
    public string? ModuleCategoryID { get; set; }
    public int IndexWithinRole { get; set; }
    public BaseCombinedStatus? RapidControlStatus { get; set; }

    public DeviceStatus() { }
}
