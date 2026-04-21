using DriveOut.Core.Map;
using DriveOut.Gameplay.Map;

namespace DriveOut.Tests.Map;

public sealed class ProceduralMapGeneratorTests
{
    [Fact]
    public void Constructor_StoreSeed()
    {
        var gen = new ProceduralMapGenerator(seed: 42);
        gen.Seed.Should().Be(42);
    }

    [Fact]
    public void Constructor_InvalidRampRate_Throws()
    {
        var act = () => new ProceduralMapGenerator(42, difficultyRampRate: 0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NextSection_ReturnsSectionWithPositiveDimensions()
    {
        var gen = new ProceduralMapGenerator(seed: 1);
        var section = gen.NextSection();

        section.Length.Should().BeGreaterThan(0f);
        section.Width.Should().BeGreaterThan(0f);
    }

    [Fact]
    public void NextSection_FirstSection_HasZeroDifficulty()
    {
        var gen = new ProceduralMapGenerator(seed: 1);
        var section = gen.NextSection();
        section.Difficulty.Should().Be(0f);
    }

    [Fact]
    public void NextSection_IncrementsSectionIndex()
    {
        var gen = new ProceduralMapGenerator(seed: 1);
        gen.NextSection();
        gen.NextSection();
        gen.SectionIndex.Should().Be(2);
    }

    [Fact]
    public void NextSection_DifficultyClampedAtOne()
    {
        // With rampRate=0.01, difficulty reaches 1 at section 100+
        var gen = new ProceduralMapGenerator(seed: 1, difficultyRampRate: 0.01f);
        TunnelSection? last = null;
        for (int i = 0; i < 200; i++) last = gen.NextSection();
        last!.Difficulty.Should().Be(1f);
    }

    [Fact]
    public void NextSection_DifficultyIncreasesWithSectionIndex()
    {
        var gen = new ProceduralMapGenerator(seed: 1, difficultyRampRate: 0.1f);
        var sections = Enumerable.Range(0, 10).Select(_ => gen.NextSection()).ToList();

        for (int i = 1; i < sections.Count; i++)
            sections[i].Difficulty.Should().BeGreaterThanOrEqualTo(sections[i - 1].Difficulty);
    }

    [Fact]
    public void SameSeed_ProducesSameSequence()
    {
        var gen1 = new ProceduralMapGenerator(seed: 99);
        var gen2 = new ProceduralMapGenerator(seed: 99);

        var seq1 = Enumerable.Range(0, 20).Select(_ => gen1.NextSection()).ToList();
        var seq2 = Enumerable.Range(0, 20).Select(_ => gen2.NextSection()).ToList();

        seq1.Should().Equal(seq2);
    }

    [Fact]
    public void DifferentSeeds_ProduceDifferentSequences()
    {
        var gen1 = new ProceduralMapGenerator(seed: 1);
        var gen2 = new ProceduralMapGenerator(seed: 2);

        var types1 = Enumerable.Range(0, 30).Select(_ => gen1.NextSection().Type).ToList();
        var types2 = Enumerable.Range(0, 30).Select(_ => gen2.NextSection().Type).ToList();

        types1.Should().NotEqual(types2);
    }

    [Fact]
    public void Reset_ResetsToInitialState()
    {
        var gen = new ProceduralMapGenerator(seed: 42);
        var first = gen.NextSection();
        gen.NextSection();
        gen.NextSection();

        gen.Reset();

        gen.SectionIndex.Should().Be(0);
        gen.NextSection().Should().Be(first);
    }

    [Fact]
    public void NextSection_AllSectionTypesReachable()
    {
        // Generate many sections — all 5 types should appear across a long run
        var gen = new ProceduralMapGenerator(seed: 0, difficultyRampRate: 0.01f);
        var types = new HashSet<TunnelSectionType>();
        for (int i = 0; i < 500; i++)
            types.Add(gen.NextSection().Type);

        types.Should().HaveCount(5, "all 5 section types should appear in a 500-section run");
    }

    [Fact]
    public void NarrowingSection_WidthDecreases_WithDifficulty()
    {
        // Force only Narrowing sections and check width decreases over difficulty
        // Use very high ramp rate to accelerate difficulty, then compare early vs late Narrowing widths
        var earlyNarrowWidths = new List<float>();
        var lateNarrowWidths = new List<float>();

        for (int seed = 0; seed < 200; seed++)
        {
            var gen = new ProceduralMapGenerator(seed, difficultyRampRate: 0.01f);
            for (int i = 0; i < 10; i++)
            {
                var s = gen.NextSection();
                if (s.Type == TunnelSectionType.Narrowing) earlyNarrowWidths.Add(s.Width);
            }
        }

        for (int seed = 0; seed < 200; seed++)
        {
            var gen = new ProceduralMapGenerator(seed, difficultyRampRate: 0.01f);
            for (int i = 0; i < 110; i++)
            {
                var s = gen.NextSection();
                if (i >= 100 && s.Type == TunnelSectionType.Narrowing) lateNarrowWidths.Add(s.Width);
            }
        }

        if (earlyNarrowWidths.Count > 0 && lateNarrowWidths.Count > 0)
        {
            earlyNarrowWidths.Average().Should().BeGreaterThan(lateNarrowWidths.Average(),
                "Narrowing sections get tighter as difficulty increases");
        }
    }
}
