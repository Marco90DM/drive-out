using DriveOut.Core.Upgrades;
using DriveOut.Gameplay.Upgrades;

namespace DriveOut.Tests.Upgrades;

public sealed class PlayerStatsTests
{
    [Fact]
    public void SpeedBoost_Apply_IncreasesMaxSpeedBonus()
    {
        var stats = new PlayerStats();
        var upgrade = new SpeedBoostUpgrade(bonus: 5f);

        upgrade.Apply(stats);

        stats.MaxSpeedBonus.Should().Be(5f);
    }

    [Fact]
    public void SpeedBoost_AppliedTwice_Stacks()
    {
        var stats = new PlayerStats();
        var upgrade = new SpeedBoostUpgrade(bonus: 5f);

        upgrade.Apply(stats);
        upgrade.Apply(stats);

        stats.MaxSpeedBonus.Should().Be(10f);
    }

    [Fact]
    public void Armor_Apply_IncreasesArmorBonus()
    {
        var stats = new PlayerStats();
        new ArmorUpgrade(bonus: 10f).Apply(stats);
        stats.ArmorBonus.Should().Be(10f);
    }

    [Fact]
    public void ScoreMultiplier_Apply_AddsToBaseMultiplier()
    {
        var stats = new PlayerStats();
        new ScoreMultiplierUpgrade(multiplierAdd: 0.5f).Apply(stats);
        stats.ScoreMultiplier.Should().BeApproximately(1.5f, 0.001f);
    }

    [Fact]
    public void HazardShield_Apply_AddsCharges()
    {
        var stats = new PlayerStats();
        new HazardShieldUpgrade(charges: 2).Apply(stats);
        stats.HazardShieldCharges.Should().Be(2);
    }

    [Fact]
    public void QuickSteer_Apply_IncreasesSteerSpeedBonus()
    {
        var stats = new PlayerStats();
        new QuickSteerUpgrade(bonus: 20f).Apply(stats);
        stats.SteerSpeedBonus.Should().Be(20f);
    }

    [Fact]
    public void Reset_ClearsAllStats()
    {
        var stats = new PlayerStats();
        new SpeedBoostUpgrade(bonus: 5f).Apply(stats);
        new ArmorUpgrade(bonus: 10f).Apply(stats);
        new ScoreMultiplierUpgrade(multiplierAdd: 0.5f).Apply(stats);
        new HazardShieldUpgrade(charges: 1).Apply(stats);

        stats.Reset();

        stats.MaxSpeedBonus.Should().Be(0f);
        stats.ArmorBonus.Should().Be(0f);
        stats.ScoreMultiplier.Should().Be(1f);
        stats.HazardShieldCharges.Should().Be(0);
        stats.SteerSpeedBonus.Should().Be(0f);
    }
}
