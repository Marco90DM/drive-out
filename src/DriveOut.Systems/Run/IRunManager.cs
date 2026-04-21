namespace DriveOut.Systems.Run;

public interface IRunManager
{
    RunState State { get; }

    event Action OnRunStarted;
    event Action OnRunPaused;
    event Action OnRunResumed;
    event Action OnGameOver;
    event Action OnRunReset;

    void StartRun();
    void Pause();
    void Resume();
    void TriggerGameOver();
    void Reset();
}
