using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;

namespace DriveOut.Tests.Progression;

public sealed class RunHistoryTests
{
    private static RunRecord MakeRecord(int run, float distance) =>
        RunRecord.From(run,
            new MetricsReport(distance, 60f, 10f, 0, new Dictionary<HazardType, int>(), null),
            DateTimeOffset.UtcNow);

    [Fact]
    public void Constructor_InvalidCapacity_Throws()
    {
        var act = () => new RunHistory(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Add_NullRecord_Throws()
    {
        var h = new RunHistory();
        var act = () => h.Add(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Add_IncreasesCount()
    {
        var h = new RunHistory();
        h.Add(MakeRecord(1, 100f));
        h.Count.Should().Be(1);
    }

    [Fact]
    public void Add_EvictsOldestWhenAtCapacity()
    {
        var h = new RunHistory(3);
        h.Add(MakeRecord(1, 100f));
        h.Add(MakeRecord(2, 200f));
        h.Add(MakeRecord(3, 300f));
        h.Add(MakeRecord(4, 400f));

        h.Count.Should().Be(3);
        h.Records[0].RunNumber.Should().Be(2);
    }

    [Fact]
    public void TopByDistance_ReturnsSortedDescending()
    {
        var h = new RunHistory();
        h.Add(MakeRecord(1, 300f));
        h.Add(MakeRecord(2, 100f));
        h.Add(MakeRecord(3, 200f));

        var top = h.TopByDistance(2);

        top[0].DistanceTraveled.Should().Be(300f);
        top[1].DistanceTraveled.Should().Be(200f);
    }

    [Fact]
    public void TopByDistance_InvalidCount_Throws()
    {
        var h = new RunHistory();
        var act = () => h.TopByDistance(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TopBySurvivalTime_ReturnsSortedDescending()
    {
        var h = new RunHistory();
        h.Add(RunRecord.From(1, new MetricsReport(100f, 90f, 0f, 0, new Dictionary<HazardType, int>(), null)));
        h.Add(RunRecord.From(2, new MetricsReport(100f, 30f, 0f, 0, new Dictionary<HazardType, int>(), null)));

        var top = h.TopBySurvivalTime(1);
        top[0].TimeSurvived.Should().Be(90f);
    }

    [Fact]
    public void PersonalBest_EmptyHistory_ReturnsNull()
    {
        new RunHistory().PersonalBest().Should().BeNull();
    }

    [Fact]
    public void PersonalBest_ReturnsFurthestRun()
    {
        var h = new RunHistory();
        h.Add(MakeRecord(1, 500f));
        h.Add(MakeRecord(2, 1000f));
        h.Add(MakeRecord(3, 750f));

        h.PersonalBest()!.DistanceTraveled.Should().Be(1000f);
    }
}
