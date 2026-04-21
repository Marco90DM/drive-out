namespace DriveOut.Core.Save;

public sealed class SaveData
{
    public int TotalRuns { get; set; }
    public float AllTimeMaxDistance { get; set; }
    public float AllTimeTotalDistance { get; set; }
    public int TotalUpgradesCollected { get; set; }
    public List<string> UnlockedUpgradeTypes { get; set; } = [];
    public List<RunRecordDto> RunHistory { get; set; } = [];
}

public sealed class RunRecordDto
{
    public int RunNumber { get; set; }
    public float DistanceTraveled { get; set; }
    public float TimeSurvived { get; set; }
    public float TotalDamageTaken { get; set; }
    public int UpgradesCollected { get; set; }
    public string? DeathCause { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
}
