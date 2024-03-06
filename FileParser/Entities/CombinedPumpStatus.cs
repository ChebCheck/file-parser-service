namespace FileParser.Entities;

[Serializable]
public class CombinedPumpStatus : BaseCombinedStatus
{
    public string? ModuleState { get; set; }
    public bool IsBusy { get; set; }
    public bool IsReady { get; set; }
    public bool IsError { get; set; }
    public bool KeyLock { get; set; }
    public string Mode { get; set; }
    public int Flow { get; set; }
    public float PercentB { get; set; }
    public float PercentC { get; set; }
    public float PercentD { get; set; }
    public double MinimumPressureLimit { get; set; }
    public double MaximumPressureLimit { get; set; }
    public double Pressure { get; set; }
    public bool PumpOn { get; set; }
    public int Channel { get; set; }

    public CombinedPumpStatus() { }
}
