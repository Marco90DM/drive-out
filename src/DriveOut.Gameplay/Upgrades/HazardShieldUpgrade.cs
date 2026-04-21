using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class HazardShieldUpgrade : IUpgrade
{
    private readonly int _charges;

    public UpgradeType Type => UpgradeType.HazardShield;
    public UpgradeRarity Rarity { get; }
    public string Name => "Hazard Shield";

    public HazardShieldUpgrade(UpgradeRarity rarity = UpgradeRarity.Rare, int charges = 1)
    {
        if (charges <= 0) throw new ArgumentOutOfRangeException(nameof(charges));
        Rarity = rarity;
        _charges = charges;
    }

    public void Apply(PlayerStats stats) => stats.HazardShieldCharges += _charges;
}
