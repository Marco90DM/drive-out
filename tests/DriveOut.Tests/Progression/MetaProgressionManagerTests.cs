using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;
using DriveOut.Core.Upgrades;
using DriveOut.Gameplay.Progression;

namespace DriveOut.Tests.Progression;

public sealed class MetaProgressionManagerTests
{
    private static MetricsReport MakeReport(float distance = 200f) =>
        new(distance, 60f, 10f, 1, new Dictionary<HazardType, int>(), "Test");

    [Fact]
    public void CompleteRun_UpdatesProgressAndHistory()
    {
        var mgr = new MetaProgressionManager();
        mgr.CompleteRun(MakeReport(300f));

        mgr.Progress.TotalRuns.Should().Be(1);
        mgr.History.Count.Should().Be(1);
    }

    [Fact]
    public void CompleteRun_NullReport_Throws()
    {
        var mgr = new MetaProgressionManager();
        var act = () => mgr.CompleteRun(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CompleteRun_FiresOnRunRecorded()
    {
        var mgr = new MetaProgressionManager();
        RunRecord? received = null;
        mgr.OnRunRecorded += r => received = r;

        mgr.CompleteRun(MakeReport(150f));

        received.Should().NotBeNull();
        received!.DistanceTraveled.Should().Be(150f);
    }

    [Fact]
    public void RegisterUnlock_ConditionMet_FiresOnUpgradeUnlocked()
    {
        var mgr = new MetaProgressionManager();
        mgr.RegisterUnlock(UpgradeType.SpeedBoost, new RunCountCondition(1));
        UpgradeType? unlocked = null;
        mgr.OnUpgradeUnlocked += t => unlocked = t;

        mgr.CompleteRun(MakeReport());

        unlocked.Should().Be(UpgradeType.SpeedBoost);
    }

    [Fact]
    public void RegisterUnlock_ConditionNotMet_DoesNotFire()
    {
        var mgr = new MetaProgressionManager();
        mgr.RegisterUnlock(UpgradeType.Armor, new RunCountCondition(5));
        UpgradeType? unlocked = null;
        mgr.OnUpgradeUnlocked += t => unlocked = t;

        mgr.CompleteRun(MakeReport());

        unlocked.Should().BeNull();
    }

    [Fact]
    public void RegisterUnlock_SameType_UnlockedOnlyOnce()
    {
        var mgr = new MetaProgressionManager();
        mgr.RegisterUnlock(UpgradeType.HazardShield, new RunCountCondition(1));
        int fireCount = 0;
        mgr.OnUpgradeUnlocked += _ => fireCount++;

        mgr.CompleteRun(MakeReport());
        mgr.CompleteRun(MakeReport());

        fireCount.Should().Be(1);
    }

    [Fact]
    public void RegisterUnlock_DistanceCondition_UnlocksWhenMet()
    {
        var mgr = new MetaProgressionManager();
        mgr.RegisterUnlock(UpgradeType.QuickSteer, new MaxDistanceCondition(500f));
        UpgradeType? unlocked = null;
        mgr.OnUpgradeUnlocked += t => unlocked = t;

        mgr.CompleteRun(MakeReport(300f));
        unlocked.Should().BeNull();

        mgr.CompleteRun(MakeReport(600f));
        unlocked.Should().Be(UpgradeType.QuickSteer);
    }

    [Fact]
    public void RegisterUnlock_NullCondition_Throws()
    {
        var mgr = new MetaProgressionManager();
        var act = () => mgr.RegisterUnlock(UpgradeType.Armor, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void History_RecordsCorrectRunNumber()
    {
        var mgr = new MetaProgressionManager();
        mgr.CompleteRun(MakeReport());
        mgr.CompleteRun(MakeReport());
        mgr.History.Records[1].RunNumber.Should().Be(2);
    }
}
