using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class HazardSpawnManager
{
    private readonly float _baseInterval;
    private readonly float _minimumInterval;
    private readonly float _difficultyRampRate;
    private readonly Random _random;
    private readonly List<HazardType> _pool;

    private float _timer;
    private float _currentInterval;

    public event Action<HazardType>? OnHazardSpawn;

    public HazardSpawnManager(
        IEnumerable<HazardType> pool,
        float baseInterval = 5f,
        float minimumInterval = 1.5f,
        float difficultyRampRate = 0.02f,
        int? seed = null)
    {
        _pool = pool.ToList();
        if (_pool.Count == 0)
            throw new ArgumentException("Spawn pool cannot be empty.", nameof(pool));
        if (baseInterval <= 0f) throw new ArgumentOutOfRangeException(nameof(baseInterval));
        if (minimumInterval <= 0f || minimumInterval >= baseInterval)
            throw new ArgumentOutOfRangeException(nameof(minimumInterval));

        _baseInterval = baseInterval;
        _minimumInterval = minimumInterval;
        _difficultyRampRate = difficultyRampRate;
        _currentInterval = baseInterval;
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public float CurrentInterval => _currentInterval;

    public void Update(float deltaTime)
    {
        if (deltaTime <= 0f) return;

        _timer += deltaTime;
        _currentInterval = Math.Max(_minimumInterval, _currentInterval - _difficultyRampRate * deltaTime);

        if (_timer < _currentInterval) return;

        _timer = 0f;
        var type = _pool[_random.Next(_pool.Count)];
        OnHazardSpawn?.Invoke(type);
    }

    public void Reset()
    {
        _timer = 0f;
        _currentInterval = _baseInterval;
    }
}
