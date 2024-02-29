namespace FileParser.Entities;

public class CombinedPumpStatus : BaseCombinedStatus
{
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


    public CombinedPumpStatus(
        BaseCombinedStatus baseStatus,
        string mode, 
        int flow,
        float percentB,
        float percentC,
        float percentD, 
        double minimumPressureLimit, 
        double maximumPressureLimit, 
        double pressure, 
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
