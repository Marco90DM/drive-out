namespace DriveOut.Core.Upgrades;

public interface IUpgrade
{
    UpgradeType Type { get; }
    UpgradeRarity Rarity { get; }
    string Name { get; }
    void Apply(PlayerStats stats);
}
