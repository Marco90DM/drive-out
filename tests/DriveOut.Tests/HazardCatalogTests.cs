using DriveOut.Core.Hazards;
using DriveOut.Gameplay.Hazards;
using DriveOut.Gameplay.Vehicle;

namespace DriveOut.Tests;

public class HazardCatalogTests
{
    private static VehicleController Vehicle() => new();

    // ── EngineOil ──────────────────────────────────────────────────────────────

    [Fact]
    public void EngineOil_Type_IsCorrect() =>
        new EngineOilHazard().Type.Should().Be(HazardType.EngineOil);

    [Fact]
    public void EngineOil_OnHit_AppliesSlowToTarget()
    {
        var hazard = new EngineOilHazard(slowMultiplier: 0.5f, duration: 3f);
        var vehicle = Vehicle();
        // Give the vehicle some speed first
        vehicle.Update(2f, new DriveOut.Core.Vehicle.VehicleInput(1f, 0f, 0f));

        hazard.OnHit(vehicle);

        // After slow, max achievable speed is halved — advance within the 3s window
        vehicle.Update(0.1f, new DriveOut.Core.Vehicle.VehicleInput(1f, 0f, 0f));
        vehicle.State.Speed.Should().BeLessOrEqualTo(vehicle.MaxSpeed * 0.5f + 1f);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(1f)]
    [InlineData(1.1f)]
    public void EngineOil_InvalidMultiplier_Throws(float m) =>
        ((Action)(() => new EngineOilHazard(m))).Should().Throw<ArgumentOutOfRangeException>();

    // ── Cardboard ─────────────────────────────────────────────────────────────

    [Fact]
    public void Cardboard_Type_IsCorrect() =>
        new CardboardHazard().Type.Should().Be(HazardType.Cardboard);

    [Fact]
    public void Cardboard_Effect_IsDamage() =>
        new CardboardHazard().Effect.EffectType.Should().Be(HazardEffectType.Damage);

    [Fact]
    public void Cardboard_DefaultDamage_Is5() =>
        new CardboardHazard().Effect.Magnitude.Should().BeApproximately(5f, 0.001f);

    [Fact]
    public void Cardboard_InvalidDamage_Throws() =>
        ((Action)(() => new CardboardHazard(0f))).Should().Throw<ArgumentOutOfRangeException>();

    // ── Spikes ────────────────────────────────────────────────────────────────

    [Fact]
    public void Spikes_Type_IsCorrect() =>
        new SpikesHazard().Type.Should().Be(HazardType.Spikes);

    [Fact]
    public void Spikes_DefaultDamage_IsHigherThanCardboard()
    {
        var spikes = new SpikesHazard();
        var cardboard = new CardboardHazard();
        spikes.Effect.Magnitude.Should().BeGreaterThan(cardboard.Effect.Magnitude);
    }

    // ── LightFlashing ─────────────────────────────────────────────────────────

    [Fact]
    public void LightFlashing_Type_IsCorrect() =>
        new LightFlashingHazard().Type.Should().Be(HazardType.LightFlashing);

    [Fact]
    public void LightFlashing_Effect_IsVisibilityImpair() =>
        new LightFlashingHazard().Effect.EffectType.Should().Be(HazardEffectType.VisibilityImpair);

    [Fact]
    public void LightFlashing_InvalidDuration_Throws() =>
        ((Action)(() => new LightFlashingHazard(0f))).Should().Throw<ArgumentOutOfRangeException>();
}
