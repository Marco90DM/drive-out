using DriveOut.Core.Difficulty;

namespace DriveOut.Gameplay.Difficulty;

public sealed class DifficultyScaler
{
    private readonly DifficultyConfig _start;
    private readonly DifficultyConfig _peak;
    private readonly float _rampDistance;

    public DifficultyConfig StartConfig => _start;
    public DifficultyConfig PeakConfig => _peak;
    public float RampDistance => _rampDistance;

    public DifficultyScaler(
        DifficultyConfig start,
        DifficultyConfig peak,
        float rampDistance = 1000f)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(peak);
        if (rampDistance <= 0f) throw new ArgumentOutOfRangeException(nameof(rampDistance));

        _start = start;
        _peak = peak;
        _rampDistance = rampDistance;
    }

    public DifficultyConfig GetConfig(float distance)
    {
        if (distance < 0f) distance = 0f;
        float t = Math.Clamp(distance / _rampDistance, 0f, 1f);
        return Lerp(_start, _peak, t);
    }

    private static DifficultyConfig Lerp(DifficultyConfig a, DifficultyConfig b, float t) => new()
    {
        HazardSpawnInterval = a.HazardSpawnInterval + (b.HazardSpawnInterval - a.HazardSpawnInterval) * t,
        HazardSpeedMultiplier = a.HazardSpeedMultiplier + (b.HazardSpeedMultiplier - a.HazardSpeedMultiplier) * t,
        BossHealthMultiplier = a.BossHealthMultiplier + (b.BossHealthMultiplier - a.BossHealthMultiplier) * t,
        UpgradeDropRate = a.UpgradeDropRate + (b.UpgradeDropRate - a.UpgradeDropRate) * t,
        PlayerDamageMultiplier = a.PlayerDamageMultiplier + (b.PlayerDamageMultiplier - a.PlayerDamageMultiplier) * t,
    };
}
