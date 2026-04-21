using DriveOut.Core.Upgrades;
using DriveOut.Gameplay.Upgrades;

namespace DriveOut.Tests.Upgrades;

public sealed class UpgradeRarityTests
{
    [Theory]
    [InlineData(UpgradeRarity.Common)]
    [InlineData(UpgradeRarity.Uncommon)]
    [InlineData(UpgradeRarity.Rare)]
    [InlineData(UpgradeRarity.Legendary)]
    public void SpeedBoost_PreservesRarity(UpgradeRarity rarity)
    {
        var upgrade = new SpeedBoostUpgrade(rarity);
        upgrade.Rarity.Should().Be(rarity);
    }

    [Fact]
    public void SpeedBoost_InvalidBonus_Throws()
    {
        var act = () => new SpeedBoostUpgrade(bonus: 0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Armor_InvalidBonus_Throws()
    {
        var act = () => new ArmorUpgrade(bonus: -5f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void HazardShield_InvalidCharges_Throws()
    {
        var act = () => new HazardShieldUpgrade(charges: 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ScoreMultiplier_InvalidMultiplier_Throws()
    {
        var act = () => new ScoreMultiplierUpgrade(multiplierAdd: -1f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
