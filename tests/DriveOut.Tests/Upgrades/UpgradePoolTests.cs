using DriveOut.Core.Upgrades;
using DriveOut.Gameplay.Upgrades;

namespace DriveOut.Tests.Upgrades;

public sealed class UpgradePoolTests
{
    private static UpgradePool BuildDefaultPool(int seed = 42)
    {
        var pool = new UpgradePool(seed);
        pool.Add(new SpeedBoostUpgrade(UpgradeRarity.Common, 5f));
        pool.Add(new ArmorUpgrade(UpgradeRarity.Common, 10f));
        pool.Add(new QuickSteerUpgrade(UpgradeRarity.Uncommon, 20f));
        pool.Add(new ScoreMultiplierUpgrade(UpgradeRarity.Rare, 0.5f));
        pool.Add(new HazardShieldUpgrade(UpgradeRarity.Legendary, 1));
        return pool;
    }

    [Fact]
    public void Add_IncreasesTotalCount()
    {
        var pool = new UpgradePool(42);
        pool.Add(new SpeedBoostUpgrade());
        pool.TotalCount.Should().Be(1);
    }

    [Fact]
    public void OfferChoices_ReturnsRequestedCount()
    {
        var pool = BuildDefaultPool();
        var choices = pool.OfferChoices(3);
        choices.Should().HaveCount(3);
    }

    [Fact]
    public void OfferChoices_ReturnsUniqueTypes()
    {
        var pool = BuildDefaultPool();
        var choices = pool.OfferChoices(5);
        choices.Select(u => u.Type).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void OfferChoices_CannotExceedDistinctTypesInPool()
    {
        var pool = new UpgradePool(42);
        pool.Add(new SpeedBoostUpgrade());
        pool.Add(new ArmorUpgrade());

        var choices = pool.OfferChoices(10);
        choices.Should().HaveCountLessOrEqualTo(2);
    }

    [Fact]
    public void OfferChoices_EmptyPool_ReturnsEmpty()
    {
        var pool = new UpgradePool(42);
        var choices = pool.OfferChoices(3);
        choices.Should().BeEmpty();
    }

    [Fact]
    public void OfferChoices_InvalidCount_Throws()
    {
        var pool = BuildDefaultPool();
        var act = () => pool.OfferChoices(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void OfferChoices_WithSameSeed_ReturnsSameResults()
    {
        var pool1 = BuildDefaultPool(seed: 99);
        var pool2 = BuildDefaultPool(seed: 99);

        var choices1 = pool1.OfferChoices(3).Select(u => u.Type).ToList();
        var choices2 = pool2.OfferChoices(3).Select(u => u.Type).ToList();

        choices1.Should().Equal(choices2);
    }

    [Fact]
    public void OfferChoices_WithDifferentSeeds_LikelyReturnsDifferentResults()
    {
        // With 5 upgrade types and 3 choices, same seed always produces same order.
        // Different seeds should produce different results across many iterations.
        var pool1 = BuildDefaultPool(seed: 1);
        var pool2 = BuildDefaultPool(seed: 2);

        var results1 = Enumerable.Range(0, 20).Select(_ => pool1.OfferChoices(3).Select(u => u.Type).ToList()).ToList();
        var results2 = Enumerable.Range(0, 20).Select(_ => pool2.OfferChoices(3).Select(u => u.Type).ToList()).ToList();

        results1.Should().NotEqual(results2);
    }

    [Theory]
    [InlineData(1000)]
    public void OfferChoices_RarityDistribution_RespresentsWeights(int iterations)
    {
        // Legendary (5%) should appear far less than Common (60%)
        var commonCount = 0;
        var legendaryCount = 0;

        for (int i = 0; i < iterations; i++)
        {
            var pool = BuildDefaultPool(seed: i);
            var choices = pool.OfferChoices(1);
            if (choices.Count == 0) continue;
            var rarity = choices[0].Rarity;
            if (rarity == UpgradeRarity.Common) commonCount++;
            if (rarity == UpgradeRarity.Legendary) legendaryCount++;
        }

        commonCount.Should().BeGreaterThan(legendaryCount * 5,
            "Common (60%) should appear at least 5x more often than Legendary (5%)");
    }
}
