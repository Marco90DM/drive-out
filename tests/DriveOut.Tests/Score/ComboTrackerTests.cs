using DriveOut.Gameplay.Score;

namespace DriveOut.Tests.Score;

public sealed class ComboTrackerTests
{
    [Fact]
    public void Initial_StreakZero_MultiplierOne()
    {
        var c = new ComboTracker();
        c.CurrentStreak.Should().Be(0);
        c.Multiplier.Should().Be(1f);
    }

    [Fact]
    public void RecordDodge_IncrementsStreak()
    {
        var c = new ComboTracker();
        c.RecordDodge();
        c.RecordDodge();
        c.CurrentStreak.Should().Be(2);
    }

    [Fact]
    public void RecordHit_ResetsStreak()
    {
        var c = new ComboTracker();
        c.RecordDodge();
        c.RecordDodge();
        c.RecordHit();
        c.CurrentStreak.Should().Be(0);
    }

    [Fact]
    public void RecordHit_WithZeroStreak_DoesNotFireEvent()
    {
        var c = new ComboTracker();
        bool fired = false;
        c.OnStreakBroken += _ => fired = true;
        c.RecordHit();
        fired.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 1f)]
    [InlineData(1, 1f)]
    [InlineData(2, 1f)]
    [InlineData(3, 1.5f)]
    [InlineData(5, 1.5f)]
    [InlineData(6, 2f)]
    [InlineData(9, 2f)]
    [InlineData(10, 3f)]
    [InlineData(14, 3f)]
    [InlineData(15, 5f)]
    [InlineData(20, 5f)]
    public void Multiplier_MatchesThresholds(int streak, float expected)
    {
        var c = new ComboTracker();
        for (int i = 0; i < streak; i++) c.RecordDodge();
        c.Multiplier.Should().Be(expected);
    }

    [Fact]
    public void OnStreakChanged_FiredOnDodge()
    {
        var c = new ComboTracker();
        (int streak, float mult) received = default;
        c.OnStreakChanged += (s, m) => received = (s, m);
        c.RecordDodge();
        received.streak.Should().Be(1);
        received.mult.Should().Be(1f);
    }

    [Fact]
    public void OnStreakChanged_FiredOnHit_WithZeroAndOne()
    {
        var c = new ComboTracker();
        var events = new List<(int, float)>();
        c.OnStreakChanged += (s, m) => events.Add((s, m));
        c.RecordDodge();   // → (1, 1f)
        c.RecordDodge();   // → (2, 1f)
        c.RecordDodge();   // → (3, 1.5f)
        c.RecordHit();     // → (0, 1f)

        events.Last().Should().Be((0, 1f));
    }

    [Fact]
    public void OnStreakBroken_PassesPreviousStreak()
    {
        var c = new ComboTracker();
        int broken = 0;
        c.OnStreakBroken += s => broken = s;
        for (int i = 0; i < 7; i++) c.RecordDodge();
        c.RecordHit();
        broken.Should().Be(7);
    }

    [Fact]
    public void Reset_ClearsStreak()
    {
        var c = new ComboTracker();
        for (int i = 0; i < 10; i++) c.RecordDodge();
        c.Reset();
        c.CurrentStreak.Should().Be(0);
        c.Multiplier.Should().Be(1f);
    }

    [Fact]
    public void MultiplierCaps_AfterFifteenDodges()
    {
        var c = new ComboTracker();
        for (int i = 0; i < 30; i++) c.RecordDodge();
        c.Multiplier.Should().Be(5f);
    }
}
