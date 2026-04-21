using DriveOut.Systems.Timer;

namespace DriveOut.Tests.Timer;

public sealed class GameTimerTests
{
    [Fact]
    public void Constructor_InvalidDuration_Throws()
    {
        var act = () => new GameTimer(0f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Initial_NotRunning_NotCompleted()
    {
        var t = new GameTimer(5f);
        t.IsRunning.Should().BeFalse();
        t.IsCompleted.Should().BeFalse();
        t.Remaining.Should().Be(5f);
        t.Elapsed.Should().Be(0f);
    }

    [Fact]
    public void Start_SetsRunning()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.IsRunning.Should().BeTrue();
    }

    [Fact]
    public void Tick_WhenNotStarted_DoesNotDecrement()
    {
        var t = new GameTimer(5f);
        t.Tick(1f);
        t.Remaining.Should().Be(5f);
    }

    [Fact]
    public void Tick_DecrementsRemaining()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.Tick(2f);
        t.Remaining.Should().BeApproximately(3f, 0.001f);
        t.Elapsed.Should().BeApproximately(2f, 0.001f);
    }

    [Fact]
    public void Tick_CompletesWhenRemainingReachesZero()
    {
        var t = new GameTimer(2f);
        t.Start();
        t.Tick(3f);
        t.Remaining.Should().Be(0f);
        t.IsRunning.Should().BeFalse();
        t.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Tick_FiresOnCompleted()
    {
        var t = new GameTimer(1f);
        bool fired = false;
        t.OnCompleted += () => fired = true;
        t.Start();
        t.Tick(1.5f);
        fired.Should().BeTrue();
    }

    [Fact]
    public void OnCompleted_FiredOnce_EvenWithOvershooting()
    {
        var t = new GameTimer(1f);
        int count = 0;
        t.OnCompleted += () => count++;
        t.Start();
        t.Tick(5f);
        t.Tick(5f);
        count.Should().Be(1);
    }

    [Fact]
    public void Pause_StopsDecrement()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.Tick(1f);
        t.Pause();
        t.Tick(2f);
        t.Remaining.Should().BeApproximately(4f, 0.001f);
    }

    [Fact]
    public void Resume_ContinuesFromPaused()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.Tick(2f);
        t.Pause();
        t.Resume();
        t.Tick(1f);
        t.Remaining.Should().BeApproximately(2f, 0.001f);
    }

    [Fact]
    public void Restart_ResetsAndRunsFromFull()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.Tick(3f);
        t.Restart();
        t.Remaining.Should().Be(5f);
        t.IsRunning.Should().BeTrue();
    }

    [Fact]
    public void Reset_ResetsToFullAndStops()
    {
        var t = new GameTimer(5f);
        t.Start();
        t.Tick(3f);
        t.Reset();
        t.Remaining.Should().Be(5f);
        t.IsRunning.Should().BeFalse();
        t.IsCompleted.Should().BeFalse();
    }
}
