using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;

namespace DriveOut.Core.Progression;

public sealed record RunRecord(
    int RunNumber,
    float DistanceTraveled,
    float TimeSurvived,
    float TotalDamageTaken,
    int UpgradesCollected,
    IReadOnlyDictionary<HazardType, int> HazardHitsByType,
    string? DeathCause,
    DateTimeOffset CompletedAt)
{
    public static RunRecord From(int runNumber, MetricsReport report, DateTimeOffset? completedAt = null) =>
        new(runNumber,
            report.DistanceTraveled,
            report.TimeSurvived,
            report.TotalDamageTaken,
            report.UpgradesCollected,
            report.HazardHitsByType,
            report.DeathCause,
            completedAt ?? DateTimeOffset.UtcNow);
}
