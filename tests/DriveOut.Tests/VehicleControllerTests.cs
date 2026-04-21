using DriveOut.Core.Vehicle;
using DriveOut.Gameplay.Vehicle;

namespace DriveOut.Tests;

public class VehicleControllerTests
{
    private static VehicleController Default() => new(maxSpeed: 30f, acceleration: 15f, deceleration: 20f);

    [Fact]
    public void InitialState_AllZero()
    {
        var sut = Default();
        sut.State.Speed.Should().Be(0f);
        sut.State.SteerAngle.Should().Be(0f);
        sut.State.LateralVelocity.Should().Be(0f);
        sut.State.IsDrifting.Should().BeFalse();
    }

    [Fact]
    public void Update_WithThrottle_IncreasesSpeed()
    {
        var sut = Default();
        sut.Update(1f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        sut.State.Speed.Should().BeGreaterThan(0f);
    }

    [Fact]
    public void Update_SpeedDoesNotExceedMaxSpeed()
    {
        var sut = Default();
        for (var i = 0; i < 100; i++)
            sut.Update(1f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        sut.State.Speed.Should().BeLessOrEqualTo(sut.MaxSpeed);
    }

    [Fact]
    public void Update_WithBrake_DecreasesSpeed()
    {
        var sut = Default();
        sut.Update(2f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        var speedBefore = sut.State.Speed;
        sut.Update(1f, new VehicleInput(Throttle: 0f, Brake: 1f, Steer: 0f));
        sut.State.Speed.Should().BeLessThan(speedBefore);
    }

    [Fact]
    public void Update_SpeedDoesNotGoBelowZero()
    {
        var sut = Default();
        for (var i = 0; i < 10; i++)
            sut.Update(1f, new VehicleInput(Throttle: 0f, Brake: 1f, Steer: 0f));
        sut.State.Speed.Should().Be(0f);
    }

    [Fact]
    public void Update_WithSteer_ChangesSteerAngle()
    {
        var sut = Default();
        sut.Update(1f, new VehicleInput(Throttle: 0f, Brake: 0f, Steer: 1f));
        sut.State.SteerAngle.Should().BeGreaterThan(0f);
    }

    [Fact]
    public void ApplyDrift_SetsDriftStateAndLateralVelocity()
    {
        var sut = Default();
        sut.Update(1f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        sut.ApplyDrift(0.8f, 3f);

        sut.State.IsDrifting.Should().BeTrue();
        sut.State.DriftFactor.Should().BeApproximately(0.8f, 0.001f);
        sut.State.DriftTimeRemaining.Should().BeApproximately(3f, 0.001f);
    }

    [Fact]
    public void ApplyDrift_WithInvalidValues_DoesNothing()
    {
        var sut = Default();
        sut.ApplyDrift(0f, 3f);
        sut.State.IsDrifting.Should().BeFalse();
    }

    [Fact]
    public void Drift_ExpiresAfterDuration()
    {
        var sut = Default();
        sut.ApplyDrift(0.8f, 1f);
        sut.Update(1.5f, new VehicleInput(Throttle: 0f, Brake: 0f, Steer: 0f));
        sut.State.IsDrifting.Should().BeFalse();
    }

    [Fact]
    public void ApplySlow_ReducesEffectiveMaxSpeed()
    {
        var sut = Default();
        sut.ApplySlow(0.5f, 10f);
        // 40 × 0.1s = 4s — slow dura 10s, siamo ben dentro la finestra
        for (var i = 0; i < 40; i++)
            sut.Update(0.1f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        sut.State.Speed.Should().BeLessOrEqualTo(sut.MaxSpeed * 0.5f + 0.01f);
    }

    [Fact]
    public void Reset_ClearsAllState()
    {
        var sut = Default();
        sut.Update(2f, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 1f));
        sut.ApplyDrift(0.8f, 5f);
        sut.Reset();

        sut.State.Speed.Should().Be(0f);
        sut.State.SteerAngle.Should().Be(0f);
        sut.State.LateralVelocity.Should().Be(0f);
        sut.State.IsDrifting.Should().BeFalse();
    }
}
