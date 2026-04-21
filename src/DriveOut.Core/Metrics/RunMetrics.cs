using DriveOut.Core.Hazards;

namespace DriveOut.Core.Metrics;

public sealed class RunMetrics
{
    private readonly Dictionary<HazardType, int> _hazardHits = [];

    public float DistanceTraveled { get; private set; }
    public float TimeSurvived { get; private set; }
    public float TotalDamageTaken { get; private set; }
    public int UpgradesCollected { get; private set; }
    public string? DeathCause { get; private set; }
    public bool IsDead => DeathCause is not null;

    public IReadOnlyDictionary<HazardType, int> HazardHitsByType => _hazardHits;

    public int TotalHazardsHit => _hazardHits.Values.Sum();

    public void RecordDistance(float delta)
    {
        if (delta <= 0f) return;
        DistanceTraveled += delta;
    }

    public void RecordTime(float delta)
    {
        if (delta <= 0f) return;
        TimeSurvived += delta;
    }

    public void RecordHazardHit(HazardType type)
    {
        _hazardHits.TryGetValue(type, out int count);
        _hazardHits[type] = count + 1;
    }

    public void RecordDamageTaken(float amount)
    {
        if (amount <= 0f) return;
        TotalDamageTaken += amount;
    }

    public void RecordUpgradeCollected() => UpgradesCollected++;

    public void RecordDeath(string cause)
    {
        if (string.IsNullOrWhiteSpace(cause)) throw new ArgumentException("Death cause cannot be empty.", nameof(cause));
        DeathCause = cause;
    }

    public MetricsReport GetReport() => new(
        DistanceTraveled,
        TimeSurvived,
        TotalDamageTaken,
        UpgradesCollected,
        new Dictionary<HazardType, int>(_hazardHits),
        DeathCause);

    public void Reset()
    {
        DistanceTraveled = 0f;
        TimeSurvived = 0f;
        TotalDamageTaken = 0f;
        UpgradesCollected = 0;
        DeathCause = null;
        _hazardHits.Clear();
    }
}
