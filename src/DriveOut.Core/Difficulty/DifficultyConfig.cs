namespace DriveOut.Core.Difficulty;

public sealed record DifficultyConfig
{
    public float HazardSpawnInterval { get; init; }
    public float HazardSpeedMultiplier { get; init; }
    public float BossHealthMultiplier { get; init; }
    public float UpgradeDropRate { get; init; }
    public float PlayerDamageMultiplier { get; init; }

    public static readonly DifficultyConfig Easy = new()
    {
        HazardSpawnInterval = 7f,
        HazardSpeedMultiplier = 0.75f,
        BossHealthMultiplier = 0.75f,
        UpgradeDropRate = 0.35f,
        PlayerDamageMultiplier = 0.75f
    };

    public static readonly DifficultyConfig Normal = new()
    {
        HazardSpawnInterval = 5f,
        HazardSpeedMultiplier = 1f,
        BossHealthMultiplier = 1f,
        UpgradeDropRate = 0.25f,
        PlayerDamageMultiplier = 1f
    };

    public static readonly DifficultyConfig Hard = new()
    {
        HazardSpawnInterval = 3f,
        HazardSpeedMultiplier = 1.3f,
        BossHealthMultiplier = 1.5f,
        UpgradeDropRate = 0.2f,
        PlayerDamageMultiplier = 1.25f
    };

    public static readonly DifficultyConfig Nightmare = new()
    {
        HazardSpawnInterval = 1.5f,
        HazardSpeedMultiplier = 1.7f,
        BossHealthMultiplier = 2f,
        UpgradeDropRate = 0.15f,
        PlayerDamageMultiplier = 1.5f
    };

    public static DifficultyConfig ForLevel(DifficultyLevel level) => level switch
    {
        DifficultyLevel.Easy => Easy,
        DifficultyLevel.Normal => Normal,
        DifficultyLevel.Hard => Hard,
        DifficultyLevel.Nightmare => Nightmare,
        _ => Normal
    };
}
