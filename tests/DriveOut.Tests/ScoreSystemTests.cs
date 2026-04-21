using DriveOut.Core.Score;
using FluentAssertions;

namespace DriveOut.Tests;

public class ScoreSystemTests
{
    [Fact]
    public void InitialState_IsZero()
    {
        var sut = new ScoreSystem();
        sut.Score.Should().Be(0);
        sut.Distance.Should().Be(0f);
    }

    [Fact]
    public void AddScore_AccumulatesCorrectly()
    {
        var sut = new ScoreSystem();
        sut.AddScore(100);
        sut.AddScore(50);
        sut.Score.Should().Be(150);
    }

    [Fact]
    public void AddScore_FiresOnScoreChangedWithTotal()
    {
        var sut = new ScoreSystem();
        int received = 0;
        sut.OnScoreChanged += v => received = v;

        sut.AddScore(200);

        received.Should().Be(200);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void AddScore_WithNonPositiveValue_DoesNothing(int points)
    {
        var sut = new ScoreSystem();
        sut.AddScore(points);
        sut.Score.Should().Be(0);
    }

    [Fact]
    public void AddDistance_AccumulatesCorrectly()
    {
        var sut = new ScoreSystem();
        sut.AddDistance(10.5f);
        sut.AddDistance(4.5f);
        sut.Distance.Should().BeApproximately(15f, 0.001f);
    }

    [Fact]
    public void AddDistance_FiresOnDistanceChangedWithTotal()
    {
        var sut = new ScoreSystem();
        float received = 0f;
        sut.OnDistanceChanged += v => received = v;

        sut.AddDistance(42f);

        received.Should().BeApproximately(42f, 0.001f);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    public void AddDistance_WithNonPositiveValue_DoesNothing(float delta)
    {
        var sut = new ScoreSystem();
        sut.AddDistance(delta);
        sut.Distance.Should().Be(0f);
    }

    [Fact]
    public void Reset_ClearsBothScoreAndDistance()
    {
        var sut = new ScoreSystem();
        sut.AddScore(500);
        sut.AddDistance(100f);

        sut.Reset();

        sut.Score.Should().Be(0);
        sut.Distance.Should().Be(0f);
    }
}
