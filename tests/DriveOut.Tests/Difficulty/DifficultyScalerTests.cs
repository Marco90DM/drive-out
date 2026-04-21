using DriveOut.Core.Difficulty;
using DriveOut.Gameplay.Difficulty;

namespace DriveOut.Tests.Difficulty;

public sealed class DifficultyScalerTests
{
    private static DifficultyScaler MakeScaler(float ramp = 1000f) =>
        new(DifficultyConfig.Easy, DifficultyConfig.Nightmare, ramp);

    [Fact]
    public void Constructor_NullStart_Throws()
    {
        var act = () => new DifficultyScaler(null!, DifficultyConfig.Normal);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_NullPeak_Throws()
    {
        var act = () => new DifficultyScaler(DifficultyConfig.Normal, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_ZeroRamp_Throws()
    {
        var act = () => new DifficultyScaler(DifficultyConfig.Easy, DifficultyConfig.Normal, 0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetConfig_AtZeroDistance_ReturnsStart()
    {
        var scaler = MakeScaler();
        var config = scaler.GetConfig(0f);
        config.Should().Be(DifficultyConfig.Easy);
    }

    [Fact]
    public void GetConfig_AtRampDistance_ReturnsPeak()
    {
        var scaler = MakeScaler(ramp: 500f);
        var config = scaler.GetConfig(500f);
        config.Should().Be(DifficultyConfig.Nightmare);
    }

    [Fact]
    public void GetConfig_BeyondRamp_ClampsToPeak()
    {
        var scaler = MakeScaler(ramp: 500f);
        var config = scaler.GetConfig(9999f);
        config.Should().Be(DifficultyConfig.Nightmare);
    }

    [Fact]
    public void GetConfig_NegativeDistance_TreatedAsZero()
    {
        var scaler = MakeScaler();
        scaler.GetConfig(-100f).Should().Be(DifficultyConfig.Easy);
    }

    [Fact]
    public void GetConfig_Midpoint_InterpolatesSpawnInterval()
    {
        var scaler = new DifficultyScaler(DifficultyConfig.Easy, DifficultyConfig.Nightmare, 1000f);
        var mid = scaler.GetConfig(500f);
        float expected = (DifficultyConfig.Easy.HazardSpawnInterval + DifficultyConfig.Nightmare.HazardSpawnInterval) / 2f;
        mid.HazardSpawnInterval.Should().BeApproximately(expected, 0.001f);
    }

    [Fact]
    public void GetConfig_SpawnIntervalDecreases_AsDistanceGrows()
    {
        var scaler = MakeScaler();
        var early = scaler.GetConfig(100f);
        var late = scaler.GetConfig(800f);
        early.HazardSpawnInterval.Should().BeGreaterThan(late.HazardSpawnInterval);
    }

    [Fact]
    public void GetConfig_DamageMultiplierIncreases_AsDistanceGrows()
    {
        var scaler = MakeScaler();
        scaler.GetConfig(100f).PlayerDamageMultiplier
            .Should().BeLessThan(scaler.GetConfig(900f).PlayerDamageMultiplier);
    }
}
