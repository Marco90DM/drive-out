using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;
using DriveOut.Core.Save;
using DriveOut.Core.Upgrades;
using DriveOut.Infrastructure.Save;

namespace DriveOut.Tests.Save;

public sealed class SaveSystemTests
{
    // --- MemorySaveService ---

    [Fact]
    public void MemorySave_InitiallyEmpty()
    {
        var svc = new MemorySaveService();
        svc.Exists.Should().BeFalse();
        svc.Load().Should().BeNull();
    }

    [Fact]
    public void MemorySave_SaveAndLoad_RoundTrips()
    {
        var svc = new MemorySaveService();
        var data = new SaveData { TotalRuns = 5, AllTimeMaxDistance = 1200f };
        svc.Save(data);
        svc.Exists.Should().BeTrue();
        svc.Load()!.TotalRuns.Should().Be(5);
        svc.Load()!.AllTimeMaxDistance.Should().Be(1200f);
    }

    [Fact]
    public void MemorySave_Delete_ClearsData()
    {
        var svc = new MemorySaveService();
        svc.Save(new SaveData());
        svc.Delete();
        svc.Exists.Should().BeFalse();
        svc.Load().Should().BeNull();
    }

    [Fact]
    public void MemorySave_NullData_Throws()
    {
        var svc = new MemorySaveService();
        var act = () => svc.Save(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    // --- SaveMapper.ToSaveData ---

    [Fact]
    public void ToSaveData_SerializesProgressFields()
    {
        var progress = new MetaProgress();
        progress.RecordRun(MakeReport(500f, 2));
        progress.Unlock(UpgradeType.SpeedBoost);
        var history = new RunHistory();
        history.Add(RunRecord.From(1, MakeReport(500f)));

        var data = SaveMapper.ToSaveData(progress, history);

        data.TotalRuns.Should().Be(1);
        data.AllTimeMaxDistance.Should().Be(500f);
        data.TotalUpgradesCollected.Should().Be(2);
        data.UnlockedUpgradeTypes.Should().Contain("SpeedBoost");
        data.RunHistory.Should().HaveCount(1);
    }

    [Fact]
    public void ToSaveData_NullProgress_Throws()
    {
        var act = () => SaveMapper.ToSaveData(null!, new RunHistory());
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToSaveData_NullHistory_Throws()
    {
        var act = () => SaveMapper.ToSaveData(new MetaProgress(), null!);
        act.Should().Throw<ArgumentNullException>();
    }

    // --- SaveMapper.ApplyToHistory ---

    [Fact]
    public void ApplyToHistory_RestoresRecords()
    {
        var progress = new MetaProgress();
        progress.RecordRun(MakeReport(300f));
        var history = new RunHistory();
        history.Add(RunRecord.From(1, MakeReport(300f)));

        var data = SaveMapper.ToSaveData(progress, history);

        var freshHistory = new RunHistory();
        SaveMapper.ApplyToHistory(data, freshHistory);

        freshHistory.Count.Should().Be(1);
        freshHistory.Records[0].DistanceTraveled.Should().Be(300f);
    }

    private static MetricsReport MakeReport(float distance, int upgrades = 0) =>
        new(distance, 60f, 10f, upgrades, new Dictionary<HazardType, int>(), null);
}
