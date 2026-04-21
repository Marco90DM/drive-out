using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class SpeedBoostUpgrade : IUpgrade
{
    private readonly float _bonus;

    public UpgradeType Type => UpgradeType.SpeedBoost;
    public UpgradeRarity Rarity { get; }
    public string Name => "Speed Boost";

    public SpeedBoostUpgrade(UpgradeRarity rarity = UpgradeRarity.Common, float bonus = 5f)
    {
        if (bonus <= 0f) throw new ArgumentOutOfRangeException(nameof(bonus));
        Rarity = rarity;
        _bonus = bonus;
    }

    public void Apply(PlayerStats stats) => stats.MaxSpeedBonus += _bonus;
}
