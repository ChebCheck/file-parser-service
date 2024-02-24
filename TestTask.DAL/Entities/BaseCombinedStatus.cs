namespace TestTask.DAL.Entities;

public class BaseCombinedStatus
{
    public string ModuleState { get; set; }
    public bool IsBusy { get; set; }
    public bool IsReady { get; set; }
    public bool IsError { get; set; }
    public bool KeyLock { get; set; }

    public BaseCombinedStatus(string moduleState, bool isBusy, bool isReady, bool isError, bool keyLock)
    {
        ModuleState = moduleState;
        IsBusy = isBusy;
        IsReady = isReady;
        IsError = isError;
        KeyLock = keyLock;
    }
}
