namespace FileParser.Entities;

public class BaseCombinedStatus
{
    public string? ModuleState { get; set; }
    public bool IsBusy { get; set; }
    public bool IsReady { get; set; }
    public bool IsError { get; set; }
    public bool KeyLock { get; set; }

    public BaseCombinedStatus() { }

    public BaseCombinedStatus(BaseCombinedStatus status)
    {
        ModuleState = status.ModuleState;
        IsBusy = status.IsBusy;
        IsReady = status.IsReady;
        IsError = status.IsError;
        KeyLock = status.KeyLock;
    }
}
