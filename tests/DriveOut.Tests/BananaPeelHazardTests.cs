using DriveOut.Core.Hazards;
using DriveOut.Gameplay.Hazards;
using DriveOut.Gameplay.Vehicle;

namespace DriveOut.Tests;

public class BananaPeelHazardTests
{
    [Fact]
    public void Type_IsBananaPeel()
    {
        var sut = new BananaPeelHazard();
        sut.Type.Should().Be(HazardType.BananaPeel);
    }

    [Fact]
    public void Effect_HasDriftType()
    {
        var sut = new BananaPeelHazard();
        sut.Effect.EffectType.Should().Be(HazardEffectType.Drift);
    }

    [Fact]
    public void Constructor_WithDefaultValues_UsesExpectedDefaults()
    {
        var sut = new BananaPeelHazard();
        sut.Effect.Magnitude.Should().BeApproximately(0.85f, 0.001f);
        sut.Effect.Duration.Should().BeApproximately(2.5f, 0.001f);
    }

    [Theory]
    [InlineData(0f, 2f)]
    [InlineData(-0.1f, 2f)]
    [InlineData(1.1f, 2f)]
    public void Constructor_WithInvalidDriftFactor_Throws(float factor, float duration)
    {
        var act = () => new BananaPeelHazard(factor, duration);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithZeroDuration_Throws()
    {
        var act = () => new BananaPeelHazard(0.8f, 0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void OnHit_AppliesDriftToTarget()
    {
        var sut = new BananaPeelHazard(driftFactor: 0.9f, duration: 3f);
        var vehicle = new VehicleController();

        sut.OnHit(vehicle);

        vehicle.State.IsDrifting.Should().BeTrue();
        vehicle.State.DriftFactor.Should().BeApproximately(0.9f, 0.001f);
        vehicle.State.DriftTimeRemaining.Should().BeApproximately(3f, 0.001f);
    }

    [Fact]
    public void OnHit_CalledTwice_RefreshesDriftDuration()
    {
        var sut = new BananaPeelHazard(driftFactor: 0.8f, duration: 3f);
        var vehicle = new VehicleController();

        sut.OnHit(vehicle);
        vehicle.Update(1f, new DriveOut.Core.Vehicle.VehicleInput(0f, 0f, 0f));
        sut.OnHit(vehicle);

        vehicle.State.DriftTimeRemaining.Should().BeApproximately(3f, 0.001f);
    }
}
