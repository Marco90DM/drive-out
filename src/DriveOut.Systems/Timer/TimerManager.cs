namespace DriveOut.Systems.Timer;

public sealed class TimerManager
{
    private readonly Dictionary<string, GameTimer> _timers = [];

    public IReadOnlyDictionary<string, GameTimer> Timers => _timers;
    public int Count => _timers.Count;

    public GameTimer Create(string name, float duration)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
        var timer = new GameTimer(duration);
        _timers[name] = timer;
        return timer;
    }

    public GameTimer? Get(string name) =>
        _timers.TryGetValue(name, out var t) ? t : null;

    public bool Remove(string name) => _timers.Remove(name);

    public void TickAll(float deltaTime)
    {
        foreach (var timer in _timers.Values)
            timer.Tick(deltaTime);
    }

    public void Clear() => _timers.Clear();
}
