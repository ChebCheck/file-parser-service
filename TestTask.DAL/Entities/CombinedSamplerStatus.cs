namespace TestTask.DAL.Entities;

public class CombinedSamplerStatus : BaseCombinedStatus
{
    public int Status { get; set; }
    public string Vial {  get; set; }
    public int Volume { get; set; }
    public int MaximumInjectionVolume { get; set; }
    public string RackL { get; set; }
    public string RackR { get; set; }
    public int RackInf { get; set; }
    public bool Buzzer { get; set; }

    public CombinedSamplerStatus(
        BaseCombinedStatus baseStatus,
        int status,
        string vial,
        int volume,
        int maximumInjectionVolume,
        string rackL,
        string rackR,
        int rackInf,
        bool buzzer) : base(baseStatus)
    {
        Status = status;
        Vial = vial;
        Volume = volume;
        MaximumInjectionVolume = maximumInjectionVolume;
        RackL = rackL;
        RackR = rackR;
        RackInf = rackInf;
        Buzzer = buzzer;
    }
}
