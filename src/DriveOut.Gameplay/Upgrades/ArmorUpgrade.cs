using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class ArmorUpgrade : IUpgrade
{
    private readonly float _bonus;

    public UpgradeType Type => UpgradeType.Armor;
    public UpgradeRarity Rarity { get; }
    public string Name => "Armor";

    public ArmorUpgrade(UpgradeRarity rarity = UpgradeRarity.Common, float bonus = 10f)
    {
        if (bonus <= 0f) throw new ArgumentOutOfRangeException(nameof(bonus));
        Rarity = rarity;
        _bonus = bonus;
    }

    public void Apply(PlayerStats stats) => stats.ArmorBonus += _bonus;
}
