using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class QuickSteerUpgrade : IUpgrade
{
    private readonly float _bonus;

    public UpgradeType Type => UpgradeType.QuickSteer;
    public UpgradeRarity Rarity { get; }
    public string Name => "Quick Steer";

    public QuickSteerUpgrade(UpgradeRarity rarity = UpgradeRarity.Uncommon, float bonus = 20f)
    {
        if (bonus <= 0f) throw new ArgumentOutOfRangeException(nameof(bonus));
        Rarity = rarity;
        _bonus = bonus;
    }

    public void Apply(PlayerStats stats) => stats.SteerSpeedBonus += _bonus;
}
