using DriveOut.Core.Metrics;
using DriveOut.Core.Upgrades;

namespace DriveOut.Core.Progression;

public sealed class MetaProgress
{
    private readonly HashSet<UpgradeType> _unlockedUpgrades = [];

    public int TotalRuns { get; private set; }
    public float AllTimeMaxDistance { get; private set; }
    public float AllTimeTotalDistance { get; private set; }
    public int TotalUpgradesCollected { get; private set; }
    public IReadOnlySet<UpgradeType> UnlockedUpgrades => _unlockedUpgrades;

    public void RecordRun(MetricsReport report)
    {
        ArgumentNullException.ThrowIfNull(report);
        TotalRuns++;
        AllTimeTotalDistance += report.DistanceTraveled;
        if (report.DistanceTraveled > AllTimeMaxDistance)
            AllTimeMaxDistance = report.DistanceTraveled;
        TotalUpgradesCollected += report.UpgradesCollected;
    }

    public bool Unlock(UpgradeType type) => _unlockedUpgrades.Add(type);

    public bool IsUnlocked(UpgradeType type) => _unlockedUpgrades.Contains(type);
}
