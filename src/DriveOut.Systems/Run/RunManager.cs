namespace DriveOut.Systems.Run;

public sealed class RunManager : IRunManager
{
    public RunState State { get; private set; } = RunState.New;

    public event Action? OnRunStarted;
    public event Action? OnRunPaused;
    public event Action? OnRunResumed;
    public event Action? OnGameOver;
    public event Action? OnRunReset;

    public void StartRun()
    {
        if (State != RunState.New)
            throw new InvalidOperationException($"Cannot start run from state {State}. Call Reset() first.");

        State = RunState.Running;
        OnRunStarted?.Invoke();
    }

    public void Pause()
    {
        if (State != RunState.Running)
            throw new InvalidOperationException($"Cannot pause from state {State}.");

        State = RunState.Paused;
        OnRunPaused?.Invoke();
    }

    public void Resume()
    {
        if (State != RunState.Paused)
            throw new InvalidOperationException($"Cannot resume from state {State}.");

        State = RunState.Running;
        OnRunResumed?.Invoke();
    }

    public void TriggerGameOver()
    {
        if (State != RunState.Running && State != RunState.Paused)
            throw new InvalidOperationException($"Cannot trigger game over from state {State}.");

        State = RunState.GameOver;
        OnGameOver?.Invoke();
    }

    public void Reset()
    {
        State = RunState.New;
        OnRunReset?.Invoke();
    }
}
