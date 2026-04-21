namespace DriveOut.Core.Progression;

public sealed class RunHistory
{
    public const int DefaultCapacity = 100;

    private readonly List<RunRecord> _records = [];
    private readonly int _capacity;

    public IReadOnlyList<RunRecord> Records => _records;
    public int Count => _records.Count;

    public RunHistory(int capacity = DefaultCapacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        _capacity = capacity;
    }

    public void Add(RunRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        _records.Add(record);
        if (_records.Count > _capacity)
            _records.RemoveAt(0);
    }

    public IReadOnlyList<RunRecord> TopByDistance(int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        return _records.OrderByDescending(r => r.DistanceTraveled).Take(count).ToList();
    }

    public IReadOnlyList<RunRecord> TopBySurvivalTime(int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        return _records.OrderByDescending(r => r.TimeSurvived).Take(count).ToList();
    }

    public RunRecord? PersonalBest() =>
        _records.Count == 0 ? null : _records.MaxBy(r => r.DistanceTraveled);
}
