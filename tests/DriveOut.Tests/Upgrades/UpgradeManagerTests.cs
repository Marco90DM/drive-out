using DriveOut.Core.Upgrades;
using DriveOut.Gameplay.Upgrades;

namespace DriveOut.Tests.Upgrades;

public sealed class UpgradeManagerTests
{
    [Fact]
    public void Apply_AddsToAcquiredList()
    {
        var manager = new UpgradeManager();
        manager.Apply(new SpeedBoostUpgrade());
        manager.Acquired.Should().HaveCount(1);
    }

    [Fact]
    public void Apply_ModifiesStats()
    {
        var manager = new UpgradeManager();
        manager.Apply(new SpeedBoostUpgrade(bonus: 5f));
        manager.Stats.MaxSpeedBonus.Should().Be(5f);
    }

    [Fact]
    public void Apply_FiresOnUpgradeApplied()
    {
        var manager = new UpgradeManager();
        IUpgrade? received = null;
        manager.OnUpgradeApplied += u => received = u;

        var upgrade = new SpeedBoostUpgrade();
        manager.Apply(upgrade);

        received.Should().BeSameAs(upgrade);
    }

    [Fact]
    public void TryConsumeShieldCharge_WithCharges_DecreasesAndReturnsTrue()
    {
        var manager = new UpgradeManager();
        manager.Apply(new HazardShieldUpgrade(charges: 2));

        var result = manager.TryConsumeShieldCharge();

        result.Should().BeTrue();
        manager.Stats.HazardShieldCharges.Should().Be(1);
    }

    [Fact]
    public void TryConsumeShieldCharge_NoCharges_ReturnsFalse()
    {
        var manager = new UpgradeManager();
        manager.TryConsumeShieldCharge().Should().BeFalse();
    }

    [Fact]
    public void TryConsumeShieldCharge_ExhaustsAllCharges()
    {
        var manager = new UpgradeManager();
        manager.Apply(new HazardShieldUpgrade(charges: 1));

        manager.TryConsumeShieldCharge().Should().BeTrue();
        manager.TryConsumeShieldCharge().Should().BeFalse();
    }

    [Fact]
    public void Reset_ClearsAcquiredAndStats()
    {
        var manager = new UpgradeManager();
        manager.Apply(new SpeedBoostUpgrade(bonus: 5f));
        manager.Apply(new HazardShieldUpgrade(charges: 1));

        manager.Reset();

        manager.Acquired.Should().BeEmpty();
        manager.Stats.MaxSpeedBonus.Should().Be(0f);
        manager.Stats.HazardShieldCharges.Should().Be(0);
    }

    [Fact]
    public void MultipleUpgradesStack_Correctly()
    {
        var manager = new UpgradeManager();
        manager.Apply(new SpeedBoostUpgrade(bonus: 5f));
        manager.Apply(new SpeedBoostUpgrade(bonus: 3f));
        manager.Apply(new ArmorUpgrade(bonus: 10f));

        manager.Stats.MaxSpeedBonus.Should().Be(8f);
        manager.Stats.ArmorBonus.Should().Be(10f);
        manager.Acquired.Should().HaveCount(3);
    }
}
