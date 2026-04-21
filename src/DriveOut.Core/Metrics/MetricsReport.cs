using DriveOut.Core.Hazards;

namespace DriveOut.Core.Metrics;

public sealed record MetricsReport(
    float DistanceTraveled,
    float TimeSurvived,
    float TotalDamageTaken,
    int UpgradesCollected,
    IReadOnlyDictionary<HazardType, int> HazardHitsByType,
    string? DeathCause);
