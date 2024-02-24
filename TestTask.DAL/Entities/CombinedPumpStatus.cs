namespace TestTask.DAL.Entities;

public class CombinedPumpStatus : BaseCombinedStatus
{
    public string Mode { get; set; }
    public int Flow { get; set; }
    public int PercentB { get; set; }
    public int PercentC { get; set; }
    public int PercentD { get; set; }
    public int MinimumPressureLimit { get; set; }
    public double MaximumPressureLimit { get; set; }
    public int Pressure { get; set; }
    public bool PumpOn { get; set; }
    public int Channel { get; set; }

    public CombinedPumpStatus(
        BaseCombinedStatus baseStatus,
        string mode, 
        int flow, 
        int percentB, 
        int percentC, 
        int percentD, 
        int minimumPressureLimit, 
        double maximumPressureLimit, 
        int pressure, 
        bool pumpOn, 
        int channel) : base(baseStatus)
    {
        Mode = mode;
        Flow = flow;
        PercentB = percentB;
        PercentC = percentC;
        PercentD = percentD;
        MinimumPressureLimit = minimumPressureLimit;
        MaximumPressureLimit = maximumPressureLimit;
        Pressure = pressure;
        PumpOn = pumpOn;
        Channel = channel;
    }
}
