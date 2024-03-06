namespace FileParser.Entities;

public class CombinedOvenStatus : BaseCombinedStatus
{
    public string? ModuleState { get; set; }
    public bool IsBusy { get; set; }
    public bool IsReady { get; set; }
    public bool IsError { get; set; }
    public bool KeyLock { get; set; }
    public bool UseTemperatureControl { get; set; }
    public bool OvenOn { get; set; }
    public float Temperature_Actual { get; set; }
    public float Temperature_Room { get; set; }
    public float MaximumTemperatureLimit { get; set; }
    public int Valve_Position { get; set; }
    public int Valve_Rotations { get; set; }
    public bool Buzzer { get; set; }

    public CombinedOvenStatus() { }
}
