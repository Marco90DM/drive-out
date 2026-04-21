using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;

namespace DriveOut.Tests.Metrics;

public sealed class RunMetricsTests
{
    // --- RecordDistance ---

    [Fact]
    public void RecordDistance_AccumulatesPositiveDeltas()
    {
        var m = new RunMetrics();
        m.RecordDistance(10f);
        m.RecordDistance(15f);
        m.DistanceTraveled.Should().BeApproximately(25f, 0.001f);
    }

    [Fact]
    public void RecordDistance_IgnoresNonPositive()
    {
        var m = new RunMetrics();
        m.RecordDistance(-5f);
        m.RecordDistance(0f);
        m.DistanceTraveled.Should().Be(0f);
    }

    // --- RecordTime ---

    [Fact]
    public void RecordTime_AccumulatesPositiveDeltas()
    {
        var m = new RunMetrics();
        m.RecordTime(1f);
        m.RecordTime(2.5f);
        m.TimeSurvived.Should().BeApproximately(3.5f, 0.001f);
    }

    [Fact]
    public void RecordTime_IgnoresNonPositive()
    {
        var m = new RunMetrics();
        m.RecordTime(0f);
        m.TimeSurvived.Should().Be(0f);
    }

    // --- RecordHazardHit ---

    [Fact]
    public void RecordHazardHit_CountsPerType()
    {
        var m = new RunMetrics();
        m.RecordHazardHit(HazardType.BananaPeel);
        m.RecordHazardHit(HazardType.BananaPeel);
        m.RecordHazardHit(HazardType.Spikes);
        m.HazardHitsByType[HazardType.BananaPeel].Should().Be(2);
        m.HazardHitsByType[HazardType.Spikes].Should().Be(1);
    }

    [Fact]
    public void TotalHazardsHit_SumsAllTypes()
    {
        var m = new RunMetrics();
        m.RecordHazardHit(HazardType.BananaPeel);
        m.RecordHazardHit(HazardType.EngineOil);
        m.TotalHazardsHit.Should().Be(2);
    }

    // --- RecordDamageTaken ---

    [Fact]
    public void RecordDamageTaken_Accumulates()
    {
        var m = new RunMetrics();
        m.RecordDamageTaken(10f);
        m.RecordDamageTaken(5f);
        m.TotalDamageTaken.Should().BeApproximately(15f, 0.001f);
    }

    [Fact]
    public void RecordDamageTaken_IgnoresNonPositive()
    {
        var m = new RunMetrics();
        m.RecordDamageTaken(-1f);
        m.TotalDamageTaken.Should().Be(0f);
    }

    // --- RecordUpgradeCollected ---

    [Fact]
    public void RecordUpgradeCollected_Increments()
    {
        var m = new RunMetrics();
        m.RecordUpgradeCollected();
        m.RecordUpgradeCollected();
        m.UpgradesCollected.Should().Be(2);
    }

    // --- RecordDeath ---

    [Fact]
    public void RecordDeath_SetsCause()
    {
        var m = new RunMetrics();
        m.RecordDeath("Spikes");
        m.DeathCause.Should().Be("Spikes");
        m.IsDead.Should().BeTrue();
    }

    [Fact]
    public void RecordDeath_EmptyCause_Throws()
    {
        var m = new RunMetrics();
        var act = () => m.RecordDeath("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IsDead_BeforeDeath_IsFalse()
    {
        new RunMetrics().IsDead.Should().BeFalse();
    }

    // --- GetReport ---

    [Fact]
    public void GetReport_SnapshotIsImmutable()
    {
        var m = new RunMetrics();
        m.RecordDistance(100f);
        m.RecordHazardHit(HazardType.Cardboard);
        m.RecordDeath("Cardboard");

        var report = m.GetReport();

        // Modify after snapshot
        m.RecordDistance(999f);

        report.DistanceTraveled.Should().BeApproximately(100f, 0.001f);
        report.HazardHitsByType[HazardType.Cardboard].Should().Be(1);
        report.DeathCause.Should().Be("Cardboard");
    }

    // --- Reset ---

    [Fact]
    public void Reset_ClearsAllState()
    {
        var m = new RunMetrics();
        m.RecordDistance(500f);
        m.RecordTime(60f);
        m.RecordDamageTaken(30f);
        m.RecordUpgradeCollected();
        m.RecordHazardHit(HazardType.BananaPeel);
        m.RecordDeath("BananaPeel");

        m.Reset();

        m.DistanceTraveled.Should().Be(0f);
        m.TimeSurvived.Should().Be(0f);
        m.TotalDamageTaken.Should().Be(0f);
        m.UpgradesCollected.Should().Be(0);
        m.TotalHazardsHit.Should().Be(0);
        m.IsDead.Should().BeFalse();
    }
}
