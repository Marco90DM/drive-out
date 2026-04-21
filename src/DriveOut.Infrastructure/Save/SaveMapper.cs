using DriveOut.Core.Progression;
using DriveOut.Core.Save;
using DriveOut.Core.Upgrades;

namespace DriveOut.Infrastructure.Save;

public static class SaveMapper
{
    public static SaveData ToSaveData(MetaProgress progress, RunHistory history)
    {
        ArgumentNullException.ThrowIfNull(progress);
        ArgumentNullException.ThrowIfNull(history);

        return new SaveData
        {
            TotalRuns = progress.TotalRuns,
            AllTimeMaxDistance = progress.AllTimeMaxDistance,
            AllTimeTotalDistance = progress.AllTimeTotalDistance,
            TotalUpgradesCollected = progress.TotalUpgradesCollected,
            UnlockedUpgradeTypes = progress.UnlockedUpgrades
                .Select(t => t.ToString())
                .ToList(),
            RunHistory = history.Records
                .Select(r => new RunRecordDto
                {
                    RunNumber = r.RunNumber,
                    DistanceTraveled = r.DistanceTraveled,
                    TimeSurvived = r.TimeSurvived,
                    TotalDamageTaken = r.TotalDamageTaken,
                    UpgradesCollected = r.UpgradesCollected,
                    DeathCause = r.DeathCause,
                    CompletedAt = r.CompletedAt
                })
                .ToList()
        };
    }

    public static void ApplyToProgress(SaveData data, MetaProgress progress)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(progress);

        foreach (var raw in data.UnlockedUpgradeTypes)
        {
            if (Enum.TryParse<UpgradeType>(raw, out var type))
                progress.Unlock(type);
        }

        // Replay runs to restore counters
        var report = new Core.Metrics.MetricsReport(
            data.AllTimeMaxDistance, 0f, 0f, 0,
            new Dictionary<Core.Hazards.HazardType, int>(), null);

        // Restore fields directly via a synthetic run record approach:
        // run recorded once with all-time max distance to set the peak,
        // then total distance and upgrade count are corrected via additional calls.
        for (int i = 0; i < data.TotalRuns; i++)
        {
            float dist = i == 0 ? data.AllTimeMaxDistance : data.AllTimeTotalDistance / Math.Max(data.TotalRuns, 1);
            int upg = i == 0 ? data.TotalUpgradesCollected : 0;
            progress.RecordRun(new Core.Metrics.MetricsReport(
                dist, 0f, 0f, upg,
                new Dictionary<Core.Hazards.HazardType, int>(), null));
        }
    }

    public static void ApplyToHistory(SaveData data, RunHistory history)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(history);

        foreach (var dto in data.RunHistory)
        {
            history.Add(new RunRecord(
                dto.RunNumber,
                dto.DistanceTraveled,
                dto.TimeSurvived,
                dto.TotalDamageTaken,
                dto.UpgradesCollected,
                new Dictionary<Core.Hazards.HazardType, int>(),
                dto.DeathCause,
                dto.CompletedAt));
        }
    }
}
