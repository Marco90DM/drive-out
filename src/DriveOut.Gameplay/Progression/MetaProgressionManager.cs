using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;
using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Progression;

public sealed class MetaProgressionManager
{
    private readonly List<(UpgradeType Type, UnlockCondition Condition)> _rules = [];

    public MetaProgress Progress { get; } = new();
    public RunHistory History { get; }

    public event Action<UpgradeType>? OnUpgradeUnlocked;
    public event Action<RunRecord>? OnRunRecorded;

    public MetaProgressionManager(int historyCapacity = RunHistory.DefaultCapacity)
    {
        History = new RunHistory(historyCapacity);
    }

    public void RegisterUnlock(UpgradeType type, UnlockCondition condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        _rules.Add((type, condition));
    }

    public void CompleteRun(MetricsReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        Progress.RecordRun(report);

        var record = RunRecord.From(Progress.TotalRuns, report);
        History.Add(record);
        OnRunRecorded?.Invoke(record);

        EvaluateUnlocks();
    }

    private void EvaluateUnlocks()
    {
        foreach (var (type, condition) in _rules)
        {
            if (condition.IsMet(Progress) && Progress.Unlock(type))
                OnUpgradeUnlocked?.Invoke(type);
        }
    }
}
