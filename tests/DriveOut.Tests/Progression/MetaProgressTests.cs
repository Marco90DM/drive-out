using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;
using DriveOut.Core.Upgrades;

namespace DriveOut.Tests.Progression;

public sealed class MetaProgressTests
{
    private static MetricsReport MakeReport(float distance = 100f, int upgrades = 0) =>
        new(distance, 60f, 10f, upgrades, new Dictionary<HazardType, int>(), "Test");

    [Fact]
    public void RecordRun_IncrementsTotalRuns()
    {
        var p = new MetaProgress();
        p.RecordRun(MakeReport());
        p.TotalRuns.Should().Be(1);
    }

    [Fact]
    public void RecordRun_TracksMaxDistance()
    {
        var p = new MetaProgress();
        p.RecordRun(MakeReport(200f));
        p.RecordRun(MakeReport(100f));
        p.AllTimeMaxDistance.Should().Be(200f);
    }

    [Fact]
    public void RecordRun_AccumulatesTotalDistance()
    {
        var p = new MetaProgress();
        p.RecordRun(MakeReport(200f));
        p.RecordRun(MakeReport(300f));
        p.AllTimeTotalDistance.Should().BeApproximately(500f, 0.001f);
    }

    [Fact]
    public void RecordRun_AccumulatesUpgrades()
    {
        var p = new MetaProgress();
        p.RecordRun(MakeReport(upgrades: 3));
        p.RecordRun(MakeReport(upgrades: 2));
        p.TotalUpgradesCollected.Should().Be(5);
    }

    [Fact]
    public void RecordRun_NullReport_Throws()
    {
        var p = new MetaProgress();
        var act = () => p.RecordRun(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Unlock_NewType_ReturnsTrue()
    {
        var p = new MetaProgress();
        p.Unlock(UpgradeType.SpeedBoost).Should().BeTrue();
    }

    [Fact]
    public void Unlock_DuplicateType_ReturnsFalse()
    {
        var p = new MetaProgress();
        p.Unlock(UpgradeType.SpeedBoost);
        p.Unlock(UpgradeType.SpeedBoost).Should().BeFalse();
    }

    [Fact]
    public void IsUnlocked_AfterUnlock_ReturnsTrue()
    {
        var p = new MetaProgress();
        p.Unlock(UpgradeType.Armor);
        p.IsUnlocked(UpgradeType.Armor).Should().BeTrue();
    }

    [Fact]
    public void IsUnlocked_NotUnlocked_ReturnsFalse()
    {
        new MetaProgress().IsUnlocked(UpgradeType.Armor).Should().BeFalse();
    }
}
