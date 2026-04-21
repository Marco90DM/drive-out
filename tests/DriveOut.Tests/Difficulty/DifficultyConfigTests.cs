using DriveOut.Core.Difficulty;

namespace DriveOut.Tests.Difficulty;

public sealed class DifficultyConfigTests
{
    [Theory]
    [InlineData(DifficultyLevel.Easy)]
    [InlineData(DifficultyLevel.Normal)]
    [InlineData(DifficultyLevel.Hard)]
    [InlineData(DifficultyLevel.Nightmare)]
    public void ForLevel_ReturnsNonNullConfig(DifficultyLevel level)
    {
        DifficultyConfig.ForLevel(level).Should().NotBeNull();
    }

    [Fact]
    public void Presets_HavePositiveValues()
    {
        foreach (var cfg in new[] { DifficultyConfig.Easy, DifficultyConfig.Normal, DifficultyConfig.Hard, DifficultyConfig.Nightmare })
        {
            cfg.HazardSpawnInterval.Should().BeGreaterThan(0f);
            cfg.HazardSpeedMultiplier.Should().BeGreaterThan(0f);
            cfg.BossHealthMultiplier.Should().BeGreaterThan(0f);
            cfg.UpgradeDropRate.Should().BeGreaterThan(0f);
            cfg.PlayerDamageMultiplier.Should().BeGreaterThan(0f);
        }
    }

    [Fact]
    public void Presets_HarderDifficultyHasShorterSpawnInterval()
    {
        DifficultyConfig.Easy.HazardSpawnInterval.Should().BeGreaterThan(DifficultyConfig.Normal.HazardSpawnInterval);
        DifficultyConfig.Normal.HazardSpawnInterval.Should().BeGreaterThan(DifficultyConfig.Hard.HazardSpawnInterval);
        DifficultyConfig.Hard.HazardSpawnInterval.Should().BeGreaterThan(DifficultyConfig.Nightmare.HazardSpawnInterval);
    }

    [Fact]
    public void Presets_HarderDifficultyHasHigherDamageMultiplier()
    {
        DifficultyConfig.Easy.PlayerDamageMultiplier.Should().BeLessThan(DifficultyConfig.Normal.PlayerDamageMultiplier);
        DifficultyConfig.Normal.PlayerDamageMultiplier.Should().BeLessThan(DifficultyConfig.Hard.PlayerDamageMultiplier);
        DifficultyConfig.Hard.PlayerDamageMultiplier.Should().BeLessThan(DifficultyConfig.Nightmare.PlayerDamageMultiplier);
    }

    [Fact]
    public void DifficultyConfig_ValueEquality()
    {
        var a = DifficultyConfig.Normal;
        var b = DifficultyConfig.ForLevel(DifficultyLevel.Normal);
        a.Should().Be(b);
    }
}
