namespace DriveOut.Systems.Timer;

public sealed class GameTimer
{
    private float _remaining;
    private bool _running;

    public float Duration { get; }
    public float Remaining => _remaining;
    public float Elapsed => Duration - _remaining;
    public bool IsRunning => _running;
    public bool IsCompleted => !_running && _remaining <= 0f && Elapsed > 0f;

    public event Action? OnCompleted;

    public GameTimer(float duration)
    {
        if (duration <= 0f) throw new ArgumentOutOfRangeException(nameof(duration));
        Duration = duration;
        _remaining = duration;
    }

    public void Start()
    {
        if (_running) return;
        _running = true;
    }

    public void Pause() => _running = false;

    public void Resume()
    {
        if (_remaining > 0f)
            _running = true;
    }

    public void Restart()
    {
        _remaining = Duration;
        _running = true;
    }

    public void Reset()
    {
        _remaining = Duration;
        _running = false;
    }

    public void Tick(float deltaTime)
    {
        if (!_running || deltaTime <= 0f) return;

        _remaining = Math.Max(0f, _remaining - deltaTime);
        if (_remaining == 0f)
        {
            _running = false;
            OnCompleted?.Invoke();
        }
    }
}
