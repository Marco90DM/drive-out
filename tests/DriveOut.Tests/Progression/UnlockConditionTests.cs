using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Progression;

namespace DriveOut.Tests.Progression;

public sealed class UnlockConditionTests
{
    private static MetaProgress ProgressWith(int runs = 0, float maxDist = 0f, float totalDist = 0f)
    {
        var p = new MetaProgress();
        var reports = runs > 0
            ? Enumerable.Range(0, runs).Select(_ =>
                new MetricsReport(totalDist / Math.Max(runs, 1), 60f, 0f, 0,
                    new Dictionary<HazardType, int>(), null))
            : [];
        foreach (var r in reports) p.RecordRun(r);
        // Override max distance by recording one run with maxDist
        if (maxDist > 0f)
            p.RecordRun(new MetricsReport(maxDist, 0f, 0f, 0, new Dictionary<HazardType, int>(), null));
        return p;
    }

    [Fact]
    public void RunCountCondition_InvalidRequired_Throws()
    {
        var act = () => new RunCountCondition(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void RunCountCondition_NotMet_ReturnsFalse()
    {
        var cond = new RunCountCondition(5);
        cond.IsMet(ProgressWith(runs: 3)).Should().BeFalse();
    }

    [Fact]
    public void RunCountCondition_ExactlyMet_ReturnsTrue()
    {
        var cond = new RunCountCondition(5);
        cond.IsMet(ProgressWith(runs: 5)).Should().BeTrue();
    }

    [Fact]
    public void RunCountCondition_Exceeded_ReturnsTrue()
    {
        var cond = new RunCountCondition(3);
        cond.IsMet(ProgressWith(runs: 10)).Should().BeTrue();
    }

    [Fact]
    public void MaxDistanceCondition_InvalidRequired_Throws()
    {
        var act = () => new MaxDistanceCondition(0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void MaxDistanceCondition_NotMet_ReturnsFalse()
    {
        var cond = new MaxDistanceCondition(500f);
        cond.IsMet(ProgressWith(maxDist: 300f)).Should().BeFalse();
    }

    [Fact]
    public void MaxDistanceCondition_Met_ReturnsTrue()
    {
        var cond = new MaxDistanceCondition(500f);
        cond.IsMet(ProgressWith(maxDist: 500f)).Should().BeTrue();
    }

    [Fact]
    public void TotalDistanceCondition_Met_ReturnsTrue()
    {
        var cond = new TotalDistanceCondition(100f);
        var p = new MetaProgress();
        p.RecordRun(new MetricsReport(60f, 0f, 0f, 0, new Dictionary<HazardType, int>(), null));
        p.RecordRun(new MetricsReport(60f, 0f, 0f, 0, new Dictionary<HazardType, int>(), null));
        cond.IsMet(p).Should().BeTrue();
    }
}
