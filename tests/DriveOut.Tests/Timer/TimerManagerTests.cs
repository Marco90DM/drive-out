using DriveOut.Systems.Timer;

namespace DriveOut.Tests.Timer;

public sealed class TimerManagerTests
{
    [Fact]
    public void Create_ReturnsNewTimer()
    {
        var mgr = new TimerManager();
        var t = mgr.Create("boss", 5f);
        t.Should().NotBeNull();
        t.Duration.Should().Be(5f);
    }

    [Fact]
    public void Create_EmptyName_Throws()
    {
        var mgr = new TimerManager();
        var act = () => mgr.Create("", 5f);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_OverwritesExisting()
    {
        var mgr = new TimerManager();
        mgr.Create("t", 5f);
        var second = mgr.Create("t", 10f);
        mgr.Get("t")!.Duration.Should().Be(10f);
    }

    [Fact]
    public void Get_UnknownName_ReturnsNull()
    {
        var mgr = new TimerManager();
        mgr.Get("x").Should().BeNull();
    }

    [Fact]
    public void Remove_ExistingTimer_ReturnsTrue()
    {
        var mgr = new TimerManager();
        mgr.Create("t", 5f);
        mgr.Remove("t").Should().BeTrue();
        mgr.Count.Should().Be(0);
    }

    [Fact]
    public void Remove_UnknownTimer_ReturnsFalse()
    {
        var mgr = new TimerManager();
        mgr.Remove("x").Should().BeFalse();
    }

    [Fact]
    public void TickAll_TicksAllRunningTimers()
    {
        var mgr = new TimerManager();
        var t1 = mgr.Create("a", 5f);
        var t2 = mgr.Create("b", 5f);
        t1.Start(); t2.Start();
        mgr.TickAll(2f);
        t1.Remaining.Should().BeApproximately(3f, 0.001f);
        t2.Remaining.Should().BeApproximately(3f, 0.001f);
    }

    [Fact]
    public void Clear_RemovesAllTimers()
    {
        var mgr = new TimerManager();
        mgr.Create("a", 1f);
        mgr.Create("b", 2f);
        mgr.Clear();
        mgr.Count.Should().Be(0);
    }
}
