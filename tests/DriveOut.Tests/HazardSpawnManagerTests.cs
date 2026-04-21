using DriveOut.Core.Hazards;
using DriveOut.Gameplay.Hazards;

namespace DriveOut.Tests;

public class HazardSpawnManagerTests
{
    private static HazardSpawnManager Default(int? seed = 42) =>
        new([HazardType.BananaPeel, HazardType.EngineOil], baseInterval: 5f, minimumInterval: 1f, seed: seed);

    [Fact]
    public void Constructor_EmptyPool_Throws() =>
        ((Action)(() => new HazardSpawnManager([]))).Should().Throw<ArgumentException>();

    [Fact]
    public void Update_BeforeInterval_DoesNotSpawn()
    {
        var sut = Default();
        var spawned = false;
        sut.OnHazardSpawn += _ => spawned = true;

        sut.Update(4f);

        spawned.Should().BeFalse();
    }

    [Fact]
    public void Update_AfterInterval_Spawns()
    {
        var sut = Default();
        HazardType? received = null;
        sut.OnHazardSpawn += t => received = t;

        sut.Update(5.1f);

        received.Should().NotBeNull();
    }

    [Fact]
    public void Update_SpawnedType_IsFromPool()
    {
        var pool = new[] { HazardType.Spikes };
        var sut = new HazardSpawnManager(pool, baseInterval: 2f, minimumInterval: 0.5f);
        HazardType? received = null;
        sut.OnHazardSpawn += t => received = t;

        sut.Update(2.1f);

        received.Should().Be(HazardType.Spikes);
    }

    [Fact]
    public void Update_DifficultyRamp_DecreasesInterval()
    {
        var sut = Default();
        var initialInterval = sut.CurrentInterval;

        // Advance a long time to trigger ramp
        for (var i = 0; i < 100; i++)
            sut.Update(1f);

        sut.CurrentInterval.Should().BeLessThan(initialInterval);
    }

    [Fact]
    public void Update_IntervalNeverBelowMinimum()
    {
        var sut = Default();
        for (var i = 0; i < 10000; i++)
            sut.Update(1f);
        sut.CurrentInterval.Should().BeGreaterOrEqualTo(1f);
    }

    [Fact]
    public void Reset_RestoresIntervalAndTimer()
    {
        var sut = Default();
        for (var i = 0; i < 100; i++)
            sut.Update(1f);
        sut.Reset();
        sut.CurrentInterval.Should().BeApproximately(5f, 0.001f);
    }
}
