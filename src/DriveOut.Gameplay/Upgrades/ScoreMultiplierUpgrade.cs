using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class ScoreMultiplierUpgrade : IUpgrade
{
    private readonly float _multiplierAdd;

    public UpgradeType Type => UpgradeType.ScoreMultiplier;
    public UpgradeRarity Rarity { get; }
    public string Name => "Score Multiplier";

    public ScoreMultiplierUpgrade(UpgradeRarity rarity = UpgradeRarity.Uncommon, float multiplierAdd = 0.5f)
    {
        if (multiplierAdd <= 0f) throw new ArgumentOutOfRangeException(nameof(multiplierAdd));
        Rarity = rarity;
        _multiplierAdd = multiplierAdd;
    }

    public void Apply(PlayerStats stats) => stats.ScoreMultiplier += _multiplierAdd;
}
